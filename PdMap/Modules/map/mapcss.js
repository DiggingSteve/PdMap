/// <reference path="c:\users\longjin\documents\visual studio 2015\Projects\PdMap\PdMap\js/seajs/sea.js" />
define(function (require, exports, moudles) {
    require("./maptalks.js");
    require("./maptalks.css");
    var map;
    var center;
    var layer = new maptalks.VectorLayer('vectorall')
    require("doT");
    require("layer");
    exports.init = function (div_map) {
        this.points = [];
        this.initBaseMap();
        this.drawMarker();
        this.drawTextMarker();
        this.drawLines();
        this.drawMultipleCircle();
        this.drawPic();
    };
    exports.initBaseMap = function () {
        this.map = new maptalks.Map("div_map", {
            center: [121.694444, 31.158735],
            zoom: 12,
            baseLayer: new maptalks.TileLayer("base", {
                urlTemplate: 'http://webrd0{s}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={x}&y={y}&z={z}',
                subdomains: [1, 2, 3]
            })

        });
        var customPosition = new maptalks.control.Zoom({
            'position': { 'bottom': '20', 'left': '20' },
            'slider': false,
            'zoomLevel': true
        });
        this.map.addControl(customPosition);
        map = this.map;
        layer.addTo(map);
        center = map.getCenter();
    };
    exports.drawMarker = function () {
        var layer = new maptalks.VectorLayer('vector').addTo(map);
        //图形标注
        var marker1 = new maptalks.Marker(
       map.getCenter().substract(0.009, 0),
       {
           symbol: {
               markerFile: '../Modules/images/wuluan.png',   //图片路径
               markerOpacity: 0.9,    //图片透明度
               markerWidth: { stops: [[15,30],[17,40]]},      //   图片宽度
               markerHeight: 40,     //图片高度
               markerDx: 0,          //偏移量x轴
               markerDy: 0       //偏移量y轴
           },
           draggable: true,       //是否可拖拽
           dragShadow: true,       //是否拖拽阴影
           cursor: 'pointer',     //鼠标样式
           shadowBlur: 10,          //阴影模糊度

       }
     ).addTo(layer);
        var marker2 = new maptalks.Marker(
              map.getCenter().substract(0.020, 0),
              {
                  'symbol': {
                      'markerType': 'pin',   //图形类型 ellipse 圆，cross十字，x,square方,diamond 钻石（竖着的正方形），bar，pie，pin
                      'markerFill': 'rgb(135,196,240)', //填充色
                      'markerFillOpacity': 0.9,           //填充色透明度
                      'markerLineColor': '#34495e',     //线颜色
                      'markerLineWidth': 3,             //线宽
                      'markerLineOpacity': 1,           //线透明度
                      'markerLineDasharray': [],        //意义不明
                      'markerWidth': 40,
                      'markerHeight': 40,
                      'markerDx': 0,
                      'markerDy': 0
                  },
                  draggable: true,       //是否可拖拽
                  dragShadow: true,       //是否拖拽阴影
                  cursor: 'pointer',     //鼠标样式
                  shadowBlur: 10,          //阴影模糊度
              }
            ).addTo(layer);


    };
    exports.drawTextMarker = function () {
        var layer = new maptalks.VectorLayer('vector1').addTo(map);
        var symbol = {
            'textFaceName': '"microsoft yahei",arial,sans-serif',
            'textName': '{name}',
            'textWeight': 'normal', //'bold', 'bolder'
            'textStyle': 'normal', //'italic', 'oblique'
            'textSize': 40,
            'textFont': null,
            'textFill': '#34495e',
            'textOpacity': 1,
            'textHaloFill': '#fff',
            'textHaloRadius': 5,
            'textWrapWidth': null,
            'textWrapBefore': false,
            'textWrapCharacter': '\n',
            'textLineSpacing': 0,

            'textDx': 0,
            'textDy': 0,

            'textHorizontalAlignment': 'middle', //left | middle | right | auto
            'textVerticalAlignment': 'middle',   // top | middle | bottom | auto
            'textAlign': 'center' //left | right | center | auto
        };
        var geometry = new maptalks.Marker(
          [121.485428, 31.228541],
          {
              'symbol': symbol,
              'properties': {
                  'name': 'Hello\nMapTalks'
              }
          }
        ).addTo(layer);
    };
    exports.drawMultipleCircle = function () {
        var layer = new maptalks.VectorLayer('vector2').addTo(map);

        var marker = new maptalks.Marker(
          map.getCenter().substract(0.1, 0),
          {
              'symbol': [
                {
                    'markerType': 'ellipse',
                    'markerFill': '#fff',
                    'markerFillOpacity': 1,
                    'markerWidth': 20,
                    'markerHeight': 20,
                    'markerLineWidth': 1,
         
          
              'markerLineOpacity': 1,     
                },
                {
                    'markerType': 'ellipse',
                    'markerFill': '#1bc8ff',
                    'markerFillOpacity': 0.9,
                    'markerWidth': 55,
                    'markerHeight': 55,
                    'markerLineWidth': 0
                },
                {
                    'markerType': 'ellipse',
                    'markerFill': '#0096cd',
                    'markerFillOpacity': 0.8,
                    'markerWidth': 91,
                    'markerHeight': 91,
                    'markerLineWidth': 0
                },
                {
                    'markerType': 'ellipse',
                    'markerFill': '#0096cd',
                    'markerFillOpacity': 0.3,
                    'markerWidth': 130,
                    'markerHeight': 98,
                    'markerLineWidth': 0
                },
                {
                    'markerType': 'ellipse',
                    'markerFill': '#0096cd',
                    'markerFillOpacity': 0.2,
                    'markerWidth': 135,
                    'markerHeight': 172,
                    'markerLineWidth': 1
                }
              ]
          }
        ).addTo(layer);
    };
    exports.drawLines = function () {
        var line1 = new maptalks.LineString(
        [
        [121.4594221902467, 31.241237891628657],
        [121.46371372467041, 31.242265291152066],
        [121.46727569824205, 31.238706037961997],
        [121.47019394165014, 31.24145804961012]
        ],
     {
         symbol: {
             'linePatternFile': '../Modules/images/wuluan.png',//线可以填充
             'lineWidth': 10,
         }
     }
).addTo(layer);
        var line2 = new maptalks.LineString(
     [map.getCenter().substract(0.05, 0.08), map.getCenter()],
     {
         symbol: [
           {
               'lineColor': '#000',
               'lineWidth': 10
           },
           {
               'lineColor': '#dedede',
               'lineWidth': 30,
               'lineDasharray': [10, 5, 10, 5]   //虚线
           }
         ]
     }).addTo(layer);
    };
    exports.drawPic = function () {
        var rectangle = new maptalks.Rectangle(center.add(-0.018, 0.012), 800, 700, {
            symbol: {
                lineColor: '#34495e',
                lineWidth: 2,
                polygonFill: '#34495e',
                polygonOpacity: 0.4
            }
        });
        var circle = new maptalks.Circle(center.add(0.002, 0.008), 500, {
            symbol: {
                lineColor: '#34495e',
                lineWidth: 2,
                polygonFill: '#1bbc9b',
                polygonOpacity: 0.4
            }
        });
        var sector = new maptalks.Sector(center.add(-0.013, -0.001), 900, 240, 300, {
            symbol: {
                lineColor: '#34495e',
                lineWidth: 2,
                polygonFill: 'rgb(135,196,240)',
                polygonOpacity: 0.4
            }
        });

        var ellipse = new maptalks.Ellipse(center.add(0.003, -0.005), 1000, 600, {
            symbol: {
                lineColor: '#34495e',
                lineWidth: 2,
                polygonFill: 'rgb(216,115,149)',
                polygonOpacity: 0.4
            }
        });

         layer.addGeometry([rectangle, circle, sector, ellipse])
        
    }
    exports.init();

})