/// <reference path="C:\Users\LongJin\Desktop\jqept.solution\Web.Site\Scripts/doT.js" />
define(function (require, exports, moudles) {

    require("./attachmentPreview.css");
    var preview_tpl = require("./tpl/preview.html");

    exports.AttchmentList;
    exports.init = function (attachmentList) {
        exports.AttchmentList = attachmentList;
        exports.renderBuild();
    }

    //渲染组建
    exports.renderBuild = function () {
        var preview_div = doT.template(preview_tpl)(exports.AttchmentList);
        var $preview_div = $(preview_div);
        $(document.body).append($preview_div);
        $preview_div.find("[data-event=close]").on("click", function () {
            $preview_div.remove();
        });
    };

    moudles.exports = exports;
})