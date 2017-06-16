/// <reference path="c:\users\longjin\documents\visual studio 2015\Projects\PdMap\PdMap\js/seajs/sea.js" />
/// <reference path="longjin.maptalks.js" />
define(function (require, exports, moudles) {
    //require("./maptalks.js");
    //require("./maptalks.css");
    //require("./maptalks.halolayer.js");

    require("doT");
    require("layer");
    exports.init = function (div_map) {
        this.allGeometries = [];
        this.lastShowTipGeometryId = "";
        this.points = [];
        this.initBaseMap();
        this.getData();
        this.layerArr = [];
        exports.initEvent();
        exports.loadLayerMenu();
        //  exports.initDrawTool();
        //exports.initDrawMenu();

    };
    exports.initBaseMap = function () {
        this.map = new maptalks.Map("div_map", {
            center: [121.694444, 31.158735],
            zoom: 12,
            maxZoom: 18,
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

        longjin.map.MapHelper.addSystemDefaultHaloLayer(this.map).hide();

        this.map.addControl(customPosition);

    };
    exports.getData = function () {
        AjaxRequest('DoAction.aspx', { method: 'GetTruckAndConstruction' }).then(function (data) {

            if (!data instanceof Array) {
                console.log("获取数据接口错误");
                alert("未获取到数据");
            }
            else {
                exports.createLayer(data);
            }
        });

    };
    exports.createLayer = function (dataArr) {
        dataArr.forEach(function (v) {
            var layerName = v.Key;   //根据小类分类
            var layerType = v.LayerType;//表的分类
            var points = v.Points;
            var markersLayer = new maptalks.VectorLayer(layerName);
            exports.layerArr.push({ parentType: layerType, type: layerName, layer: markersLayer });//paremtType 大类区分车队工地 ，type  小类
            points.forEach(function (pVal) {
                var id = layerName + pVal.Id;
                var marker = new maptalks.Marker([pVal.GaodeLong, pVal.GaodeLat], {
                    symbol: exports.getMarkerSymbol(pVal.PointType),
                    properties: pVal,//绑定所有属性
                    id: id
                });
                marker.addTo(markersLayer);
                marker.on("mouseover", function (evt) {
                    var props = evt.target.getProperties();
                    exports.showTipInfo(evt.target, props.Name);
                });
                marker.on('click', function () {
                    var tpl_detail;
                    if (layerType == "construction" && layerName != "进出土") {
                        tpl_detail = require("./tpl/markerDetail.html");
                    }
                    debugger;
                    if (layerType == "truck") {
                        tpl_detail = require("./tpl/truckDetail.html");
                    }
                    if (layerName == "进出土") {
                        tpl_detail = require("./tpl/transportSoilDetail.html");
                    }
                    if (layerName == "landfillWaste") {
                        tpl_detail = require("./tpl/landfillDetail.html");
                    }
                    exports.showInfoWindow(markersLayer, id, tpl_detail);
                    return false;
                });
                exports.map.on("click", function () {
                    marker.closeInfoWindow();
                });
                exports.points.push(marker);
            });

            markersLayer.addTo(exports.map);
            exports.allGeometries = exports.allGeometries.concat(markersLayer.getGeometries());
        });

    };
    exports.initDrawTool = function () {
        exports.drawLayer = new maptalks.VectorLayer('drawLayer').addTo(exports.map);

        exports.drawTool = new maptalks.DrawTool({
            mode: 'Rectangle'
        }).addTo(exports.map).disable();

        exports.drawTool.on('drawend', function (param) {
            //console.log(param.geometry);
            exports.drawLayer.addGeometry(param.geometry);
            exports.pointsInCircle = [];
            exports.points.forEach(function (v) {
                if (param.geometry.containsPoint(v._coordinates)) {
                    exports.pointsInCircle.push(v);

                }
            });
            exports.showPointsInCircle(exports.pointsInCircle);
        });

    }

    exports.initDrawMenu = function () {
        var items = ['圆'].map(function (value) {
            return {
                item: value,
                click: function () {
                    var mode;
                    switch (value) {
                        case "圆":
                            mode = "Polygon";
                            break;
                    }
                    exports.drawTool.setMode(mode).enable();
                }
            };
        });
        exports.toolbar = new maptalks.control.Toolbar({
            items: [
              {
                  item: '菜单',
                  children: items
              },
              {
                  item: '禁用画图',
                  click: function () {
                      exports.drawTool.disable();
                  }
              },
              {
                  item: '清除',
                  click: function () {
                      exports.drawLayer.clear();
                  }
              }
            ]
        }).addTo(exports.map);
    }
    //监听地图点击事件
    exports.initEvent = function () {
        //exports.onMapClick();
        exports.searchPoint();
    }
    exports.onMapClick = function () {
        this.map.on("click", function (e) {
            //console.log(e);
        });

    };
    exports.getMarkerSymbol = function (layerType) {
        var path;
        switch (layerType) {

            case "开工":
                path = "../Modules/images/开工.png";
                break;
            case "进出土":
                path = "../Modules/images/进出土.png";
                break;
            case "停工":
                path = "../Modules/images/停工.png";
                break;
            case "未验":
                path = "../Modules/images/未验.png";
                break;
            case "打桩":
                path = "../Modules/images/打桩.png";
                break;
        }
        var symbol = {
            'markerFile': path,
            'markerOpacity': 1,
            'markerWidth': 14,
            'markerHeight': 14,
            'markerDx': 0,
            'markerDy': 0
        }
        switch (layerType) {
            case "landfillWaste":

                symbol = {
                    'markerType': 'diamond',
                    'markerFill': 'rgba(113,67,15)',
                    'markerFillOpacity': 1,
                    'markerLineColor': '#34495e',
                    'markerLineWidth': 0,
                    'markerLineOpacity': 1,
                    'markerLineDasharray': [],
                    'markerWidth': 14,
                    'markerHeight': 14,
                    'markerDx': 0,
                    'markerDy': 0
                };
                break;
            case "constructionWaste":

                symbol = {
                    'markerType': 'ellipse',
                    'markerFill': '#dfb228',
                    'markerFillOpacity': 1,
                    'markerLineColor': '#34495e',
                    'markerLineWidth': 0,
                    'markerLineOpacity': 1,
                    'markerLineDasharray': [],
                    'markerWidth': 14,
                    'markerHeight': 14,
                    'markerDx': 0,
                    'markerDy': 0
                };
                break;
            case "mud":

                symbol = {
                    'markerType': 'ellipse',
                    'markerFill': '#1ca8db',
                    'markerFillOpacity': 1,
                    'markerLineColor': '#34495e',
                    'markerLineWidth': 0,
                    'markerLineOpacity': 1,
                    'markerLineDasharray': [],
                    'markerWidth': 14,
                    'markerHeight': 14,
                    'markerDx': 0,
                    'markerDy': 0
                };
                break;
            case "decorationWasteTruck":

                symbol = {
                    'markerType': 'ellipse',
                    'markerFill': '#75980b',
                    'markerFillOpacity': 1,
                    'markerLineColor': '#34495e',
                    'markerLineWidth': 0,
                    'markerLineOpacity': 1,
                    'markerLineDasharray': [],
                    'markerWidth': 14,
                    'markerHeight': 14,
                    'markerDx': 0,
                    'markerDy': 0
                };
                break;
        }
        
        return symbol
    }
    exports.drawCircle = function (layer) {
        var circle = new maptalks.Circle([121.48542888885189, 31.228541533313702], 1000, {
            id: 'circle0',
            symbol: {
                lineColor: "#000", lineOpacity: 1,
                lineWidth: 2, polygonFill: "#000", polygonOpacity: 0.3
            }
        }).addTo(layer);
        console.log(circle);
    }
    exports.loadLayerMenu = function () {
        exports.layerMenu = new maptalks.control.Toolbar({
            'height': '28px',
            'vertical': false,
            'position': 'top-right',
            'items': [
                {
                    item: '<span>车队</span>',
                    children: [{
                        item: '全部',
                        click: function () { exports.showLayerGroup("truck") }
                    },
                    {
                        item: '渣土车',
                        click: function () { exports.showLayerByType("constructionWaste") }
                    }, {
                        item: '装修垃圾车',
                        click: function () { exports.showLayerByType("decorationWasteTruck") }
                    },
                    {
                        item: '泥浆车',
                        click: function () { exports.showLayerByType("mud") }
                    }]
                },
            {
                item: '<span>工地</span>',
                children: [
                    {
                        item: '全部',
                        click: function () { exports.showLayerGroup("construction") }

                    },
                    {
                        item: '进出土',
                        click: function () { exports.showLayerByType("进出土") }
                    },
                    {
                        item: '打桩',
                        click: function () { exports.showLayerByType("打桩") }
                    },
                    {
                        item: '开工',
                        click: function () { exports.showLayerByType("开工") }

                    },
                    {
                        item: '停工',
                        click: function () { exports.showLayerByType("停工") }
                    }
                 //{
                 //    item: '未验',
                 //    click: function () { exports.showLayerByType("未验") }
                 //}
                ]
            },
            {
                item: '<span>渣土卸点</span>', click: function () {
                    exports.showLayerByType("landfillWaste")
                }
            }
            ]
        });
        exports.map.addControl(exports.layerMenu);
    };
    exports.showLayerByType = function (type) {
        var geometries = [];
        exports.layerArr.forEach(function (item) {
            if (item.type === type && item.layer instanceof maptalks.VectorLayer) geometries = geometries.concat(item.layer.getGeometries());
        });
        debugger;
        exports.showGeometries(geometries, null, true);
    };
    exports.showLayerGroup = function (parentType) {
        var geometries = [];
        this.layerArr.forEach(function (v, i) {
            if (v.parentType === parentType) {
                geometries = geometries.concat(v.layer.getGeometries());
            }
        });
        exports.showGeometries(geometries, null, false);
    };
    exports.showPointsInCircle = function (markerArr) {
        var tpl_detail = require("./tpl/selectedDetail.html");
        console.log(markerArr);
        debugger;
        openLayer(tpl_detail, markerArr);
    };
    exports.showTipInfo = function (geometry, info) {
        var tipWindow = geometry.getInfoWindow();
        if (tipWindow && geometry.getId() == exports.lastShowTipGeometryId && tipWindow.isVisible()) return;
        if (tipWindow && !!tipWindow.getTitle()) geometry.removeInfoWindow();
        if (!tipWindow) {
            tipWindow = new maptalks.ui.InfoWindow({
                content: "<div style='text-align:center;'>" + info + "</div>",
                width: 250
            });
            tipWindow.addTo(geometry);
        }
        tipWindow.show(geometry.getCenter());
        exports.lastShowTipGeometryId = geometry.getId();
        return false;
    };
    exports.showInfoWindow = function (poiLayer, id, htmlContent) {
        var geometry = poiLayer.getGeometryById(id);
        var props = geometry.getProperties();
        var tplHtml = doT.template(htmlContent)(props);
        var options = {
            'title': props.Name,
            'content': tplHtml,
            'width': 250
        };
        var center = geometry.getCenter();
        var infoWindow = new maptalks.ui.InfoWindow(options);
        infoWindow.addTo(geometry).show(center);
        //exports.map.setCenter(center);
    };
    exports.searchPoint = function () {
        $("#txt_search").on("keydown", function (e) {
            var text = $("#txt_search").val();
            if (e.keyCode == 13) {
                if ($.trim(text).length <= 0) {
                    exports.showGeometries(null, null, false);
                    return;
                }
                var type = $("#sel_type").val();
                switch (type) {
                    case "addr":
                        exports.showGeometries(null, function (item) {
                            return item.properties && item.properties.Address && item.properties.Address.indexOf(text) != -1;
                        }, true);
                        break;
                    case "side":
                        exports.showGeometries(null, function (item) {
                            return item.properties && item.properties.BuildingCompany && item.properties.BuildingCompany.indexOf(text) != -1;
                        }, true);
                        break;
                    case "name":
                    default:
                        exports.showGeometries(null, function (item) {
                            return item.properties && item.properties.Name && item.properties.Name.indexOf(text) != -1;
                        }, true);
                        break;
                }
            }
        });
    };
    exports.clearHaloPoints = function () {
        var haloLayer = exports.map.getLayer(longjin.core.constant._SYSTEM_DEFAULT_HALO_LAYER);
        if (haloLayer) {
            exports.map.removeLayer(haloLayer);
            haloLayer.clear().hide();
            haloLayer.addTo(exports.map);
        }
    };
    exports.addHaloPoints = function (coordinates) {
        if (!coordinates || !$.isArray(coordinates)) return;
        var haloLayer = exports.map.getLayer(longjin.core.constant._SYSTEM_DEFAULT_HALO_LAYER);
        exports.map.removeLayer(haloLayer);
        coordinates.forEach(function (coordinate) {
            longjin.animation.AnimationHelper.addHalo([coordinate], haloLayer, {
                id: longjin.util.Common.randomString(32),
                properties: {
                    mag: 100
                }
            });
        });
        haloLayer.addTo(exports.map).bringToBack().show();
    };
    exports.showGeometries = function (geometries, func, showHalo) {
        if (!geometries || !$.isArray(geometries)) geometries = exports.allGeometries;
        var visibleLayers = [];
        var haloCoordinates = [];
        exports.hideLayers(null);
        if (func && $.isFunction(func)) {
            geometries.forEach(function (item, index) {
                if (func(item)) {
                    if (visibleLayers.indexOf(item.getLayer().getId()) < 0) visibleLayers.push(item.getLayer().getId());
                    haloCoordinates.push(item.getCoordinates().toArray());
                    item.show();
                } else {
                    item.hide();
                }
            });

        } else {
            geometries.forEach(function (item, index) {
                if (visibleLayers.indexOf(item.getLayer().getId()) < 0) visibleLayers.push(item.getLayer().getId());
                haloCoordinates.push(item.getCoordinates().toArray());
                item.show();
            });
        }
        if (showHalo) {
            exports.clearHaloPoints();
            exports.addHaloPoints(haloCoordinates);
        }
        exports.showLayers(visibleLayers);
    };
    exports.showLayers = function (layers) {
        if (!layers || !$.isArray(layers)) return;
        exports.map.getLayers(function (item) {
            return item instanceof maptalks.VectorLayer;
        }).forEach(function (item) {
            layers.indexOf(item.getId()) >= 0 ? item.show() : item.hide();
        });
    };
    exports.hideLayers = function (layers) {
        if (!layers || !$.isArray(layers) || layers.length <= 0) layers = exports.map.getLayers();
        layers.forEach(function (item) {
            item.hide();
        });
    };

    moudles.exports = exports;
    exports.init();

    function openLayer(tpl, v) {
        var tplHtml = doT.template(tpl)(v);
        layer.open({
            type: 1, //Page层类型
            area: ['800px', '300px'],
            title: '详情',
            shade: 0.6, //遮罩透明度
            maxmin: true,//允许全屏最小化
            anim: 1, //0-6的动画形式，-1不开启
            content: tplHtml,
            cancel: function () {
            }
        });
    }
})