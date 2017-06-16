(function(){
/**
 * The Sea.js plugin for collecting HTTP requests and sending all at once
 */

var Module = seajs.Module
var load = Module.prototype.load
var data = seajs.data

var useStack = data.flushUseStack = []
var depStack = data.flushDepStack = []

var isLoadOnRequest = false
var isInUse = false


Module.prototype.load = function() {
  var mod = this

  if (needLoadImmediately(mod)) {
    load.call(mod)
  }
  else {
    isInUse ? useStack.push(mod) : depStack.push(mod)
  }
}

seajs.use = function(ids, callback) {
  isInUse = true
  Module.use(ids, callback, data.cwd + "_use_" + data.cid())
  isInUse = false

  return seajs
}

seajs.flush = function() {
  flush(useStack)
}

function flush(stack) {
  var len = stack.length
  if (len === 0) {
    return
  }

  var currentStack = stack.splice(0, len)
  var deps = []

  // Collect dependencies
  for (var i = 0; i < len; i++) {
    deps = deps.concat(currentStack[i].resolve())
  }

  // Remove duplicate and saved modules
  deps = getUnfetchedUris(deps)

  // Create an anonymous module for flushing
  var mod = Module.get(
      data.cwd + "_flush_" + data.cid(),
      deps
  )

  mod.load = load

  mod._entry.push(mod)
  mod.history = {}
  mod.remain = 1

  mod.callback = function() {
    for (var i = 0; i < len; i++) {
      currentStack[i].onload()
    }
    delete mod.callback
    delete mod.history
    delete mod.remain
    delete mod._entry
  }

  // Load it
    mod.load()
}

seajs.on("request", function(data) {
  var onRequest = data.onRequest

  // Flush to load dependencies at onRequest
  data.onRequest = function() {
    isLoadOnRequest = true
    onRequest()
    isLoadOnRequest = false

    flush(depStack)
  }
})


// Helpers

var ASYNC_RE = /\.js_async_\d+$/

function needLoadImmediately(mod) {
  return hasEmptyDependencies(mod) ||
      isSavedBeforeRequest(mod) ||
//      isPreload(mod) ||
      isAsync(mod)
}

function isSavedBeforeRequest(mod) {
  return !isLoadOnRequest && mod.status === Module.STATUS.SAVED
}

function hasEmptyDependencies(mod) {
  return mod.dependencies.length === 0
}

function isAsync(mod) {
  return ASYNC_RE.test(mod.uri)
}

function getUnfetchedUris(uris) {
  var ret = []
  var hash = {}
  var uri

  for (var i = 0, len = uris.length; i < len; i++) {
    uri = uris[i]

    // Remove duplicate uris
    if (uri && !hash[uri]) {
      hash[uri] = true

      // Remove saved modules
      if(!seajs.cache[uri] || seajs.cache[uri].status < Module.STATUS.SAVED){
        ret.push(uri)
      }
    }
  }

  return ret
}

define("seajs/seajs-flush/1.1.0/seajs-flush-debug", [], {});
})();