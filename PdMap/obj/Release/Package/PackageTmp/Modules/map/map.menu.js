/// <reference path="c:\users\longjin\documents\visual studio 2015\Projects\PdMap\PdMap\js/seajs/sea.js" />
/// <reference path="legend.js" />

define(function (require, exports, moudles) {
    var drawTool = new drawTools();
    exports.init = function () {
        exports.loadEvent();
        exports.drawLegend();
    };
    exports.loadEvent = function () {
        exports.legendBtnEvent("#btn_legend");
    };
    exports.legendBtnEvent = function (id) {
        $(id).click(function () {
            $(".legend_wrap").css("top", "10px");
        });
        $(".legend_wrap .close").click(function () {
            $(".legend_wrap").css("top", "-300px");
        });
    };

    exports.drawLegend = function () {
        drawTool.drawCircle({ id: "legend_constructionWaste", canvasWidth: 18, canvasHeight: 18, x: 9, y: 9, radius: 9, color: "#dfb228" });
        drawTool.drawCircle({ id: "legend_mud", canvasWidth: 18, canvasHeight: 18, x: 9, y: 9, radius: 9, color: "#1ca8db" });
        drawTool.drawCircle({ id: "legend_decoration", canvasWidth: 18, canvasHeight: 18, x: 9, y: 9, radius: 9, color: "#75980b" })
        drawTool.drawDiamond({ id: "legend_landfill",canvasWidth:18,canvasHeight:18,x:0,y:0,h:12,w:12,color:"#000" });
    }
    exports.init();
});