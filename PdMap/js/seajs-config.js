/// <reference path="seajs/sea.js" /
seajs.config({
    base: RootPath,
  //预加载
  preload: [
      Function.prototype.bind ? '' : 'es5-safe',
      this.JSON ? '' : 'json',
      "seajsText",
      "alert"
  ],

  // 设置路径，方便跨目录调用
  paths: {

  },

  // 设置别名，方便调用
  alias: {
    /*
        外部组件
    */
    "cookie": "Scripts/jquery.cookie-1.4.1.min.js",
    "seajsText": "js/seajs/seajs-text.js",
    "seajsCss": "js/seajs/seajs-css.js",
    "doT": "Scripts/doT.js",
    "uploader": "Modules/plug-in/uploader/uploader.js",
    "validator": "Scripts/bootstrapValidator.min",

    "alert": "js/plug-in/alert/alert.js",
    "es5Safe": "js/plug-in/es5Safe/es5Safe.js",
    "json": "js/plug-in/json/json3.js",
    "date": "js/plug-in/date/calendar.js",
    "BDMap": "js/BDMap/BDMap.js",
    "attachmentPreview": "js/plug-in/attachmentPreview/attachmentPreview.js",
      "layer":"js/layer/layer.js"

  },

  map: [
          [/^(.*\.(?:css|js|htm|html))(\?.*)?$/i, '$1?' + new Date().getTime()]
  ]
});