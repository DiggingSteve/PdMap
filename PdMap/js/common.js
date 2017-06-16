
/*
    封装StringBuilder
*/
function StringBuilder() {
    this._string_ = [];
}
StringBuilder.prototype.Append = function (str) {
    this._string_.push(str);
};
StringBuilder.prototype.toString = function () {
    return this._string_.join("");
};
StringBuilder.prototype.AppendFormat = function () {
    if (arguments.length > 1) {
        var TString = arguments[0];
        if (arguments[1] instanceof Array) {
            for (var i = 0, iLen = arguments[1].length; i < iLen; i++) {
                var jIndex = i;
                var re = new RegExp('\\{' + jIndex + '\\}', 'g');
                TString = TString.replace(re, arguments[1][i]);
            }
        } else {
            for (var j = 1, iLen = arguments.length; j < iLen; j++) {
                var jIndex = j - 1;
                var re = new RegExp('\\{' + jIndex + '\\}', 'g');
                TString = TString.replace(re, arguments[j]);
            }
        }
        this.Append(TString);
    } else if (arguments.length === 1) {
        this.Append(arguments[0]);
    }
};
StringBuilder.prototype.Length = function () {
    return this._string_.length;
};

Array.prototype.remove = function (value) {
    if (value) {
        var arry = this.valueOf();
        var index = arry.indexOf(value);
        if (index != -1) {
            arry = arry.splice(index, 1);
        }
        return this.valueOf();
    }
}

/*
    重写endsWithIE下不兼容
*/
String.prototype.endsWith = function (value) {
    var str = value.toString();
    var lastChar = this.substr(this.length - str.length, str.length);
    return str == lastChar;
};

/*
    重写startsWith IE下不兼容
*/
String.prototype.startsWith = function (value) {
    var str = value.toString();
    var firstChar = this.substr(0, str.length);
    return str == firstChar;
};

/*
    发送Ajax 请求
    @param.url　地址
    @param.param 参数
*/
function AjaxRequest(url, param) {
    return $.ajax({
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        method: "POST",
        dataType: "JSON",
        url: url,
        data: param,
        success: function (source) {
            
            if (source && !source.Status) {
                if (source.Code == 100) {
          
                    return false;
                }
                else {
                    if (source.Msg) {
                        alert(source.Msg, 2);
                    }
                    return false;
                }
            }
        },
        error: function () {
            alert("服务器异常");
        }
    });
}

/*
   检查用户是否登陆
*/
function CheckUser() {
    if (!user) {
        $("#btn_login").click();
    }
}

/*
    获取URL中的参数
    @param.name 参数名
*/
function queryString(name) {
    var reg = new RegExp("[&|?]" + name + "=([^&$]*)", "gi");
    var a = reg.test(location.search);
    return decodeURIComponent(a ? RegExp.$1 : "");
}

/*
    格式化附件大小
    @param.length 整形
*/
function fileLengthFormat(length) {
    var kb = length / 1024;
    if (kb < 1024) {
        return kb.toFixed(2) + "KB";
    }
    var mb = kb / 1024;
    return mb.toFixed(2) + "MB";
}

function htmlencode(s) {
    var div = document.createElement('div');
    div.appendChild(document.createTextNode(s));
    return div.innerHTML;
}
function htmldecode(s) {
    var div = document.createElement('div');
    div.innerHTML = s;
    return div.innerText || div.textContent;
}

/*
    设置相对母对象的相对位置
    @param.$el：指定的相对元素
    @param.$current:需要设置定位的元素
*/
function setRelativeParentPosition($el, $current) {
    var $parent = $el.parent();
    var top = $el.offset().top - $parent.offset().top;
    var left = $el.offset().left - $parent.offset().left;
    var height = $el.height();
    var width = $el.width();
    var z_index = 0;
    $el.parent().css("position", "relative");
    var $parents = $el.parents();
    for (var i = 0; i < $parents.length; i++) {
        var _position = $parents.eq(i).css("position");
        if (_position && (_position.toLowerCase() == "fixed" || _position.toLowerCase() == "absolute")) {
            var _z_index = $parents.eq(i).css("z-index");
            if (_z_index) {
                _z_index = parseInt(_z_index);
                if (_z_index > z_index) {
                    z_index = _z_index;
                }
            }
        }
    }

    $current.css({
        position: "absolute",
        top: -(10 + height) + "px",
        left: left + "px",
        "max-width": width + "px",
        "z-index": z_index,
    });
}

/*
    返回大于或等于指定的双精度浮点数的最小整数值。
*/
function ceiling(num) {
    var _num = num.toFixed();
    if (_num > num) {
        return _num;
    }
    if (_num < num) {
        return parseInt(num) + 1;
    }
    return num;
}

/*
    公共事件处理
*/
$(function () {
    //全局回车事件
    $(window).on("keyup", function (event) {
        switch (event.keyCode) {
            case 13://回车
                var $enter = $("[data-enter-click=true]");
                if ($enter.length == 1) {
                    var tagName = event.target.tagName;
                    if (["BODY", "INPUT", "BUTTON"].indexOf(tagName.toUpperCase()) != -1) {
                        $enter.click();
                    }
                    else {
                        if (tagName.toUpperCase() == "DIV") {
                            if ($(event.target).prop("contenteditable").toLowerCase() != "true") {
                                $enter.click();
                            }
                        }
                    }
                }
                break;
            case 39://→
                var $next = $("[data-next-click=true]");
                if ($next.length == 1) {
                    $next.click();
                }
                break;
            case 37://←
                var $prev = $("[data-prev-click=true]");
                if ($prev.length == 1) {
                    $prev.click();
                }
                break;
        }
    });

    $.fn.initPagination = function (total, current, call) {
        if (total <= 1) {
            return "";
        }
        var str = new StringBuilder();
        str.Append('<li class="disabled" data-page="previous"><a href="javascript:void(0)">«</a></li>');
        for (var i = 1; i <= total; i++) {
            if (current == i) {
                str.AppendFormat('<li class="active" data-page="{0}"><a href="javascript:void(0)">{0}<span class="sr-only">(current)</span></a></li>', i
                    );
            }
            else {
                str.AppendFormat('<li data-page="{0}"><a href="javascript:void(0)">{0}</a></li>', i);
            }
        }
        str.Append('<li data-page="next"><a href="javascript:void(0)">»</a></li>');

        var $page = $(str.toString());

        $(this).html($page);

        $page.on("click", function () {
            var page = 1;
            var $current = $(this);
            if ($current.hasClass("active")) {
                return;
            }
            if ($current.hasClass("disabled")) {
                return;
            }

            page = $current.attr("data-page");
            if (page == "previous") {
                page = $page.siblings(".active").attr("data-page");
                page = parseInt(page) - 1;
            } else if (page == "next") {
                page = $page.siblings(".active").attr("data-page");
                page = parseInt(page) + 1;
            }
            else {
                page = parseInt(page);
            }

            $current.siblings().removeClass("active disabled");

            if (page <= 1) {
                $page.siblings('[data-page="previous"]').addClass("disabled");
            }
            if (page >= total) {
                $page.siblings('[data-page="next"]').addClass("disabled");
            }

            $page.siblings("[data-page=" + page + "]").addClass("active");
            call(page);
        });

    };
});

//监控JS异常
window.onerror = function (msg, uri, line, column) {

};


/*
    以下部分为一个整体   currentUri和currentUri_absolutePath不能缺少
    其中currentUri_absolutePath为当前的JS文件的绝对目录
*/
var currentUri = function () {
    if (document.currentScript) {
        return document.currentScript.src;
    } else {
        var uri = document.scripts[document.scripts.length - 1];
        return uri.src;
    }
}();
var currentUri_absolutePath = "/Modules";
/*
    当前系统根目录
*/
var FullPath = function () {
    var _FullPath;
    var currentUrl_arry = window.document.location.href.split('/');
    var currentUri_arry = currentUri.split('/');
    currentUri_arry.pop();//去除文件名
    if (currentUri_absolutePath) {
        var currentUri_str = currentUri_arry.join('/');
        currentUri_str = currentUri_str.substr(0, currentUri_str.length - currentUri_absolutePath.length);
        _FullPath = currentUri_str;
    }
    else {
        _FullPath = [];
        for (var i = 0; i < currentUri_arry.length; i++) {
            var item = currentUri_arry[i];
            if (item.toLowerCase() == currentUrl_arry[i].toLowerCase()) {
                _FullPath.push(item);
                continue;
            }
            break;
        }
        _FullPath = _FullPath.join('/');
    }
    return _FullPath;
}();

/*
    把结对路径转换为客户端可用的地址
*/
var ResolveUrl = function (absolutePath) {
    return FullPath + absolutePath;
}


