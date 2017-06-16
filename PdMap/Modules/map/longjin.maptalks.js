var __assign = (this && this.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var longjin;
(function (longjin) {
    var core;
    (function (core) {
        var constant;
        (function (constant) {
            constant._EVENT_SUBMITING = "submitting";
            constant._EVENT_SUBMITED = "submitted";
            constant._SYSTEM_DEFAULT_VECTOR_LAYER = "system_default_vectorlayer";
            constant._SYSTEM_DEFAULT_CLUSTER_LAYER = "system_default_clusterlayer";
            constant._SYSTEM_DEFAULT_HEAT_LAYER = "system_default_heatlayer";
            constant._SYSTEM_DEFAULT_HALO_LAYER = "system_default_halolayer";
            constant._SYSTEM_DEFAULT_LAYER_ID_PREFIX = "layer_";
            constant._SYSTEM_DEFAULT_GEOMETRY_ID_PREFIX = "geometry_";
        })(constant = core.constant || (core.constant = {}));
    })(core = longjin.core || (longjin.core = {}));
})(longjin || (longjin = {}));
/// <reference path="../core/literal.ts" />
var longjin;
(function (longjin) {
    var util;
    (function (util) {
        //常量
        var x_PI = 3.14159265358979324 * 3000.0 / 180.0;
        var PI = 3.1415926535897932384626;
        var a = 6378245.0;
        var ee = 0.00669342162296594323;
        /*
         * 坐标转换帮助类
         */
        var CoordinateConverter = (function () {
            function CoordinateConverter() {
            }
            /*
             * 坐标转换
             */
            CoordinateConverter.fromSourceToTarget = function (coordinate, sourceType, targetType) {
                switch (sourceType) {
                    case "GaoDe":
                        switch (targetType) {
                            case "OpenStreet":
                                return CoordinateConverter.fromGCJ02ToWGS84(coordinate);
                            case "BaiDu":
                                return CoordinateConverter.fromGCJ02ToBD09(coordinate);
                            default:
                                return coordinate;
                        }
                    case "BaiDu":
                        switch (targetType) {
                            case "GaoDe":
                                return CoordinateConverter.fromBD09ToGCJ02(coordinate);
                            case "OpenStreet":
                                return CoordinateConverter.fromGCJ02ToWGS84(CoordinateConverter.fromBD09ToGCJ02(coordinate));
                            default:
                                return coordinate;
                        }
                    case "OpenStreet":
                    default:
                        switch (targetType) {
                            case "GaoDe":
                                return CoordinateConverter.fromWGS84ToGCJ02(coordinate);
                            case "BaiDu":
                                return CoordinateConverter.fromGCJ02ToBD09(CoordinateConverter.fromWGS84ToGCJ02(coordinate));
                            default:
                                return coordinate;
                        }
                }
            };
            /*
             * [集合]坐标转换
             */
            CoordinateConverter.fromSourceToTargetMulti = function (coordinates, sourceType, targetType) {
                switch (sourceType) {
                    case "GaoDe":
                        switch (targetType) {
                            case "OpenStreet":
                                return CoordinateConverter.fromGCJ02ToWGS84Multi(coordinates);
                            case "BaiDu":
                                return CoordinateConverter.fromGCJ02ToBD09Multi(coordinates);
                            default:
                                return coordinates;
                        }
                    case "BaiDu":
                        switch (targetType) {
                            case "GaoDe":
                                return CoordinateConverter.fromBD09ToGCJ02Multi(coordinates);
                            case "OpenStreet":
                                return CoordinateConverter.fromGCJ02ToWGS84Multi(CoordinateConverter.fromBD09ToGCJ02Multi(coordinates));
                            default:
                                return coordinates;
                        }
                    case "OpenStreet":
                    default:
                        switch (targetType) {
                            case "GaoDe":
                                return CoordinateConverter.fromWGS84ToGCJ02Multi(coordinates);
                            case "BaiDu":
                                return CoordinateConverter.fromGCJ02ToBD09Multi(CoordinateConverter.fromWGS84ToGCJ02Multi(coordinates));
                            default:
                                return coordinates;
                        }
                }
            };
            /*
             * 百度坐标系 (BD-09) 与 火星坐标系 (GCJ-02)的转换
             * 即 百度 转 谷歌、高德
             */
            CoordinateConverter.fromBD09ToGCJ02 = function (coordinate) {
                var bd_lng = coordinate[0];
                var bd_lat = coordinate[1];
                var x_pi = 3.14159265358979324 * 3000.0 / 180.0;
                var x = bd_lng - 0.0065;
                var y = bd_lat - 0.006;
                var z = Math.sqrt(x * x + y * y) - 0.00002 * Math.sin(y * x_pi);
                var theta = Math.atan2(y, x) - 0.000003 * Math.cos(x * x_pi);
                var gg_lng = z * Math.cos(theta);
                var gg_lat = z * Math.sin(theta);
                return [gg_lng, gg_lat];
            };
            /*
             * [集合]百度坐标系 (BD-09) 与 火星坐标系 (GCJ-02)的转换
             * 即 百度 转 谷歌、高德
             */
            CoordinateConverter.fromBD09ToGCJ02Multi = function (coordinates) {
                var arr = [];
                for (var _i = 0, coordinates_1 = coordinates; _i < coordinates_1.length; _i++) {
                    var coordinate = coordinates_1[_i];
                    arr.push(CoordinateConverter.fromBD09ToGCJ02(coordinate));
                }
                return arr;
            };
            /*
             * GCJ-02 转换为 WGS-84
             */
            CoordinateConverter.fromGCJ02ToWGS84 = function (coordinate) {
                var lng = coordinate[0];
                var lat = coordinate[1];
                if (CoordinateConverter.outOfChina(coordinate)) {
                    return [lng, lat];
                }
                else {
                    var dlat = CoordinateConverter.transformLat([lng - 105.0, lat - 35.0]);
                    var dlng = CoordinateConverter.transformLng([lng - 105.0, lat - 35.0]);
                    var radlat = lat / 180.0 * PI;
                    var magic = Math.sin(radlat);
                    magic = 1 - ee * magic * magic;
                    var sqrtmagic = Math.sqrt(magic);
                    dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                    dlng = (dlng * 180.0) / (a / sqrtmagic * Math.cos(radlat) * PI);
                    var mglat = lat + dlat;
                    var mglng = lng + dlng;
                    return [lng * 2 - mglng, lat * 2 - mglat];
                }
            };
            /*
             * [集合]GCJ-02 转换为 WGS-84
             */
            CoordinateConverter.fromGCJ02ToWGS84Multi = function (coordinates) {
                var arr = [];
                for (var _i = 0, coordinates_2 = coordinates; _i < coordinates_2.length; _i++) {
                    var coordinate = coordinates_2[_i];
                    arr.push(CoordinateConverter.fromGCJ02ToWGS84(coordinate));
                }
                return arr;
            };
            /*WGS-84转GCJ-02*/
            CoordinateConverter.fromWGS84ToGCJ02 = function (coordinate) {
                var lng = coordinate[0];
                var lat = coordinate[1];
                if (CoordinateConverter.outOfChina(coordinate)) {
                    return [lng, lat];
                }
                else {
                    var dlat = CoordinateConverter.transformLat([lng - 105.0, lat - 35.0]);
                    var dlng = CoordinateConverter.transformLng([lng - 105.0, lat - 35.0]);
                    var radlat = lat / 180.0 * PI;
                    var magic = Math.sin(radlat);
                    magic = 1 - ee * magic * magic;
                    var sqrtmagic = Math.sqrt(magic);
                    dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                    dlng = (dlng * 180.0) / (a / sqrtmagic * Math.cos(radlat) * PI);
                    var mglat = Number(lat) + Number(dlat);
                    var mglng = Number(lng) + Number(dlng);
                    return [mglng, mglat];
                }
            };
            /*
             * [集合]GCJ-02 转换为 WGS-84
             */
            CoordinateConverter.fromWGS84ToGCJ02Multi = function (coordinates) {
                var arr = [];
                for (var _i = 0, coordinates_3 = coordinates; _i < coordinates_3.length; _i++) {
                    var coordinate = coordinates_3[_i];
                    arr.push(CoordinateConverter.fromWGS84ToGCJ02(coordinate));
                }
                return arr;
            };
            /*
             * 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换
             * 即谷歌、高德 转 百度
             */
            CoordinateConverter.fromGCJ02ToBD09 = function (coordinate) {
                var lng = coordinate[0];
                var lat = coordinate[1];
                var z = Math.sqrt(lng * lng + lat * lat) + 0.00002 * Math.sin(lat * x_PI);
                var theta = Math.atan2(lat, lng) + 0.000003 * Math.cos(lng * x_PI);
                var bd_lng = z * Math.cos(theta) + 0.0065;
                var bd_lat = z * Math.sin(theta) + 0.006;
                return [bd_lng, bd_lat];
            };
            /*
             * [集合]火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换
             * 即谷歌、高德 转 百度
             */
            CoordinateConverter.fromGCJ02ToBD09Multi = function (coordinates) {
                var arr = [];
                for (var _i = 0, coordinates_4 = coordinates; _i < coordinates_4.length; _i++) {
                    var coordinate = coordinates_4[_i];
                    arr.push(CoordinateConverter.fromGCJ02ToBD09(coordinate));
                }
                return arr;
            };
            CoordinateConverter.transformLat = function (coordinate) {
                var lng = coordinate[0];
                var lat = coordinate[1];
                var ret = -100.0 + 2.0 * lng + 3.0 * lat + 0.2 * lat * lat + 0.1 * lng * lat + 0.2 * Math.sqrt(Math.abs(lng));
                ret += (20.0 * Math.sin(6.0 * lng * PI) + 20.0 * Math.sin(2.0 * lng * PI)) * 2.0 / 3.0;
                ret += (20.0 * Math.sin(lat * PI) + 40.0 * Math.sin(lat / 3.0 * PI)) * 2.0 / 3.0;
                ret += (160.0 * Math.sin(lat / 12.0 * PI) + 320 * Math.sin(lat * PI / 30.0)) * 2.0 / 3.0;
                return ret;
            };
            CoordinateConverter.transformLng = function (coordinate) {
                var lng = coordinate[0];
                var lat = coordinate[1];
                var ret = 300.0 + lng + 2.0 * lat + 0.1 * lng * lng + 0.1 * lng * lat + 0.1 * Math.sqrt(Math.abs(lng));
                ret += (20.0 * Math.sin(6.0 * lng * PI) + 20.0 * Math.sin(2.0 * lng * PI)) * 2.0 / 3.0;
                ret += (20.0 * Math.sin(lng * PI) + 40.0 * Math.sin(lng / 3.0 * PI)) * 2.0 / 3.0;
                ret += (150.0 * Math.sin(lng / 12.0 * PI) + 300.0 * Math.sin(lng / 30.0 * PI)) * 2.0 / 3.0;
                return ret;
            };
            /**
             * 判断是否在国内，不在国内则不做偏移
             * @param lng
             * @param lat
             * @returns {boolean}
             */
            CoordinateConverter.outOfChina = function (coordinate) {
                var lng = coordinate[0];
                var lat = coordinate[1];
                return (lng < 72.004 || lng > 137.8347) || ((lat < 0.8293 || lat > 55.8271) || false);
            };
            /**
             * 百度墨卡托转经纬度坐标
             * @param lng
             * @param lat
             * @param divId
             * @returns{lngLatPt}
             */
            //			baiduMercatoToLngLat(x: number, y: number, divId: string) {
            //				let baiduMap = new BMap.Map(divId);
            //				//通过web mercato坐标构建起点终点
            //				let ptXY = new BMap.Pixel(x, y);
            //				//通过相应接口将起点终点的web mercato坐标转换为经纬度坐标
            //				let projection2 = baiduMap.getMapType().getProjection();
            //				let lngLatPt = projection2.pointToLngLat(ptXY);
            //				return lngLatPt;
            //			}
            /**
             * 百度经纬度坐标转百度墨卡托坐标
             * @param lng
             * @param lat
             * @param divId
             * @returns{mercatoPt}
             */
            //			baiduLngLatToMercato(lng: number, lat: number, divId: string) {
            //				let bdXY = new BMap.Point(lng, lat);
            //				let baiduMap = new BMap.Map(divId);
            //				let projection2 = baiduMap.getMapType().getProjection();
            //				let mercatoPt = projection2.lngLatToPoint(bdXY);
            //				return mercatoPt;
            //			}
            /*
                    根据地址信息获取经纬度,返回json对象：
                        status    Int    返回结果状态值，成功返回0。
                        location object    经纬度坐标
                        lat    float    纬度值
                        lng    float    经度值
                        precise    Int    位置的附加信息，是否精确查找。1为精确查找，0为不精确。
                        confidence    Int    可信度
                        level    string    地址类型
          
                    {
                        status: 0,
                        result:
                        {
                            location:
                            {
                                lng: 116.30814954222,
                                lat: 40.056885091681
                            },
                            precise: 1,
                            confidence: 80,
                            level: "商务大厦"
                        }
                    }
                */
            CoordinateConverter.fromAddressToBDLngLatAsync = function (address, callback) {
                if ($.trim(address).length > 0) {
                    /*let url = 'http://api.map.baidu.com/geocoder/v2/?ak=eIxDStjzbtH0WtU50gqdXYCz&output=json&address=' + encodeURIComponent(address);*/
                    var url = 'http://api.map.baidu.com/geocoder/v2/?ak=4DKNgUoGZhnKScxcH8t6wMQ6&output=json&address=' + encodeURIComponent(address);
                    //根据地点名称获取经纬度信息
                    $.ajax({
                        type: "GET",
                        url: url,
                        dataType: "JSONP",
                        success: function (data) {
                            if (parseInt(data.status) == 0 && callback) {
                                callback([data.result.location.lng, data.result.location.lat]);
                            }
                        }
                    });
                }
            };
            /*
                根据经纬度获取详细地址及其周边信息，返回json对象：
                    status    constant    返回结果状态值， 成功返回0，其他值请查看附录。
                    location
                        lat    纬度坐标
                        lng    经度坐标
                    formatted_address    结构化地址信息
                    business    所在商圈信息，如 "人民大学,中关村,苏州街"
                    addressComponent
                        city    城市名
                        district    区县名
                        province    省名
                        street    街道名
                        street_number    街道门牌号
                    pois（周边poi数组）
                        addr    地址信息
                        cp    数据来源
                        distance    离坐标点距离
                        name    poi名称
                        poiType    poi类型，如’ 办公大厦,商务大厦’
                        point    poi坐标{x,y}
                        tel    电话
                        uid    poi唯一标识
                        zip    邮编
            */
            CoordinateConverter.fromDBLngLatToAddressAsync = function (coordinate, callback) {
                var lng = coordinate[0];
                var lat = coordinate[1];
                var url = 'http://api.map.baidu.com/geocoder/v2/?ak=eIxDStjzbtH0WtU50gqdXYCz&output=json&pois=1&location=' + lat + "," + lng;
                $.ajax({
                    type: "GET",
                    url: url,
                    dataType: "JSONP",
                    success: function (data) {
                        if (parseInt(data.status) == 0 && callback) {
                            callback(data.result);
                            /*let result = "地址：" + data.result.formatted_address + "</br>";
                            result += "省名:" + data.result.addressComponent.province + "</br>";
                            result += "城市名:" + data.result.addressComponent.city + "</br>";
                            result += "区县名:" + data.result.addressComponent.district + "</br>";
                            result += "街道名:" + data.result.addressComponent.street + "</br>";
                            result += "街道门牌号:" + data.result.addressComponent.street_number + "</br>";
                            result += "周边信息：</br>";
                            for(var i = 0; i < data.result.pois.length; i++) {
                                result += "地址信息:" + data.result.pois[i].addr +
                                    ",数据来源:" + data.result.pois[i].cp +
                                    ",离坐标点距离:" + data.result.pois[i].distance +
                                    ",poi名称:" + data.result.pois[i].name +
                                    ",poi类型:" + data.result.pois[i].poiType +
                                    ",poi坐标x:" + data.result.pois[i].point.x +
                                    ",poi坐标y:" + data.result.pois[i].point.y +
                                    ",电话:" + data.result.pois[i].tel +
                                    ",poi唯一标识:" + data.result.pois[i].uid +
                                    ",邮编:" + data.result.pois[i].zip + "</br>";
                            }*/
                        }
                    }
                });
            };
            return CoordinateConverter;
        }());
        util.CoordinateConverter = CoordinateConverter;
    })(util = longjin.util || (longjin.util = {}));
})(longjin || (longjin = {}));
var longjin;
(function (longjin) {
    var util;
    (function (util) {
        var Common = (function () {
            function Common() {
            }
            /**
             * 获取当前时间戳
             */
            Common.getCurrentTimestamp = function () {
                return (new Date()).valueOf();
            };
            /*
             * 生成GUID
             */
            Common.guid = function (length, radix) {
                var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
                var uuid = [];
                radix = radix || chars.length;
                if (length) {
                    for (var i = 0; i < length; i++) {
                        uuid[i] = chars[0 | Math.random() * radix];
                    }
                }
                else {
                    var r = void 0;
                    uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
                    uuid[14] = '4';
                    for (var i = 0; i < 36; i++) {
                        if (!uuid[i]) {
                            r = 0 | Math.random() * 16;
                            uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
                        }
                    }
                }
                return uuid.join('');
            };
            /**
             * 生成随机字符串
             */
            Common.randomString = function (length, radix) {
                var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
                var uuid = [];
                radix = radix || chars.length;
                if (length) {
                    for (var i = 0; i < length; i++) {
                        uuid[i] = chars[0 | Math.random() * radix];
                    }
                }
                else {
                    var r = void 0;
                    uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
                    uuid[14] = '4';
                    for (var i = 0; i < 36; i++) {
                        if (!uuid[i]) {
                            r = 0 | Math.random() * 16;
                            uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
                        }
                    }
                }
                return uuid.join('');
            };
            /*
             * 根据坐标生成ID
             */
            Common.coordinateToId = function (coordinate, prefix) {
                if (!coordinate)
                    return null;
                if (maptalks.StringUtil.trim(prefix).length <= 0)
                    prefix = "_ID_";
                return prefix + coordinate[0] + "_" + coordinate[1];
            };
            /*
             * 执行条件判断
             */
            Common.evalConditions = function (value, conditions) {
                var ret = null;
                conditions.map(function (item) {
                    if (eval(item[0].replace(/\{\$value\}/gi, (function (val) {
                        if (typeof val == "string") {
                            return "\"" + val + "\"";
                        }
                        else {
                            return val;
                        }
                    })(value)))) {
                        if (item[2] && $.isFunction(item[2])) {
                            ret = {
                                symbol: item[1],
                                callback: item[2]
                            };
                        }
                        else {
                            ret = {
                                symbol: item[1]
                            };
                        }
                    }
                });
                return ret;
            };
            return Common;
        }());
        util.Common = Common;
    })(util = longjin.util || (longjin.util = {}));
})(longjin || (longjin = {}));
/// <reference path="../jquery.d.ts" />
/// <reference path="../maptalks.d.ts" />
/// <reference path="../core/literal.ts" />
/// <reference path="../core/constant.ts" />
/// <reference path="../layer/index.d.ts" />
var longjin;
(function (longjin) {
    var map;
    (function (map_1) {
        var Map = (function () {
            function Map(container, type, options) {
                maptalks.Map.include({
                    type: '',
                    getType: function () {
                        return this.type;
                    },
                    toJSON: function (options) {
                        if (!options) {
                            options = {};
                        }
                        var profile = {
                            'version': this['PROFILE_VERSION'],
                            'extent': this.getExtent().toJSON()
                        };
                        profile['options'] = this.config();
                        profile['options']['center'] = this.getCenter();
                        profile['options']['zoom'] = this.getZoom();
                        var baseLayer = this.getBaseLayer();
                        if ((maptalks.Util.isNil(options['baseLayer']) || options['baseLayer']) && baseLayer) {
                            profile['baseLayer'] = baseLayer.toJSON(options['baseLayer']);
                        }
                        var extraLayerOptions = {};
                        if (options['clipExtent']) {
                            //if clipExtent is set, only geometries intersecting with extent will be exported.
                            //clipExtent's value can be an extent or true (map's current extent)
                            if (options['clipExtent'] === true) {
                                extraLayerOptions['clipExtent'] = this.getExtent();
                            }
                            else {
                                extraLayerOptions['clipExtent'] = options['clipExtent'];
                            }
                        }
                        var i, len, layers, opts, layersJSON = [];
                        if (maptalks.Util.isNil(options['layers']) || (options['layers'] && !maptalks.Util.isArray(options['layers']))) {
                            layers = this.getLayers();
                            for (i = 0, len = layers.length; i < len; i++) {
                                if (!layers[i].toJSON) {
                                    continue;
                                }
                                opts = maptalks.Util.extend({}, maptalks.Util.isObject(options['layers']) ? options['layers'] : {}, extraLayerOptions);
                                layersJSON.push(layers[i].toJSON(opts));
                            }
                            profile['layers'] = layersJSON;
                        }
                        else if (maptalks.Util.isArrayHasData(options['layers'])) {
                            layers = options['layers'];
                            for (i = 0; i < layers.length; i++) {
                                var exportOption = layers[i];
                                var layer = this.getLayer(exportOption['id']);
                                if (!layer.toJSON) {
                                    continue;
                                }
                                opts = maptalks.Util.extend({}, exportOption['options'], extraLayerOptions);
                                layersJSON.push(layer.toJSON(opts));
                            }
                            profile['layers'] = layersJSON;
                        }
                        else {
                            profile['layers'] = [];
                        }
                        profile['type'] = type;
                        return profile;
                    }
                });
                maptalks.Map.mergeOptions(options || {});
                var map = new maptalks.Map(container, options);
                map.type = type;
                return map;
            }
            return Map;
        }());
        map_1.Map = Map;
        /*地图帮助类*/
        var MapHelper = (function () {
            function MapHelper() {
            }
            /*加载地图*/
            MapHelper.load = function (container, type, options) {
                /*let _TypedMap = maptalks.Map.extend({
                    initialize: function(id: string, type ? : MapType, opts ? : any) {
                        //this.setId(id);
                        //maptalks.Util.setOptions(this, opts);
                        this.type = type;
                    },
                    getType(): string {
                        return this.type;
                    },
                    getDefaultVectorLayer(): any {
                        return this.getLayer(core.constant._SYSTEM_DEFAULT_VECTOR_LAYER);
                    },
                    getDefaultClusterLayer(): any {
                        return this.getLayer(core.constant._SYSTEM_DEFAULT_CLUSTER_LAYER);
                    },
                    getDefaultHeatLayer(): any {
                        return this.getLayer(core.constant._SYSTEM_DEFAULT_HEAT_LAYER);
                    },
                    getDefaultHaloLayer(): any {
                        return this.getLayer(core.constant._SYSTEM_DEFAULT_HALO_LAYER);
                    }
                });*/
                var _map = {};
                switch (type) {
                    case "GaoDe":
                        _map = new maptalks.Map(container, __assign({ center: [121.48542888885189, 31.228541533313702], zoom: 12, minZoom: 11, maxZoom: 18, view: {
                                projection: 'EPSG:3857'
                            }, baseLayer: MapHelper.getTileLayer("base", type), layers: [] }, options));
                        break;
                    case "OpenStreet":
                        _map = new maptalks.Map(container, __assign({ center: [121.48542888885189, 31.228541533313702], zoom: 12, minZoom: 11, maxZoom: 18, baseLayer: MapHelper.getTileLayer("base", type), layers: [] }, options));
                        break;
                    case "ZhiTu":
                        _map = new maptalks.Map(container, __assign({ center: [121.48542888885189, 31.228541533313702], zoom: 12, minZoom: 11, maxZoom: 18, baseLayer: MapHelper.getTileLayer("base", type), layers: [] }, options));
                        break;
                    case "LocalBaiDu":
                        _map = new maptalks.Map(container, __assign({ center: [121.436815, 31.178097], zoom: 12, minZoom: 11, maxZoom: 18, view: {
                                projection: 'baidu'
                            }, baseLayer: MapHelper.getTileLayer("base", type), layers: [] }, options));
                        break;
                    case "LocalGaoDe":
                        _map = new maptalks.Map(container, __assign({ 
                            /*center: [121.48542888885189, 31.228541533313702],*/
                            center: [121.474759, 31.235422], zoom: 12, minZoom: 11, maxZoom: 18, view: {
                                projection: 'EPSG:3857'
                            }, baseLayer: MapHelper.getTileLayer("base", type), layers: [] }, options));
                        break;
                    case "BaiDu.Midnight":
                        _map = new maptalks.Map(container, __assign({ center: [121.474759, 31.235422], zoom: 12, minZoom: 11, maxZoom: 18, view: {
                                projection: 'baidu'
                            }, baseLayer: MapHelper.getTileLayer("base", type), layers: [] }, options));
                        break;
                    case "BaiDu":
                    default:
                        _map = new maptalks.Map(container, __assign({ center: [121.474759, 31.235422], zoom: 12, minZoom: 11, maxZoom: 18, view: {
                                projection: 'baidu'
                            }, baseLayer: MapHelper.getTileLayer("base", type), layers: [] }, options));
                        break;
                }
                // 添加指定功能图层
                // MapHelper.addSystemSpecialLayers(_map);
                // 添加默认图层
                // MapHelper.addSystemDefaultVectorLayer(_map);
                return _map;
            };
            /*
             * 获取瓦片图层
             */
            MapHelper.getTileLayer = function (name, type) {
                if (maptalks.StringUtil.trim(name).length <= 0)
                    name = "base";
                switch (type) {
                    case "GaoDe":
                        return new maptalks.TileLayer(name, {
                            urlTemplate: 'http://webrd0{s}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={x}&y={y}&z={z}',
                            subdomains: [1, 2, 3]
                        });
                    case "ZhiTu":
                        return new maptalks.TileLayer(name, {
                            urlTemplate: 'http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetPurplishBlue/MapServer/tile/{z}/{y}/{x}'
                        });
                    case "OpenStreet":
                        return new maptalks.TileLayer(name, {
                            urlTemplate: 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
                            subdomains: ['a', 'b', 'c']
                        });
                    case "LocalBaiDu":
                        return new maptalks.TileLayer(name, {
                            urlTemplate: 'http://192.168.10.127:11214/tile/baidu/tile/{z}/{x}/{y}'
                        });
                    case "LocalGaoDe":
                        return new maptalks.TileLayer(name, {
                            'urlTemplate': 'http://192.168.10.127:11214/tile/gaode/tile/{z}/{x}/{y}'
                        });
                    case "BaiDu.Midnight":
                        return new maptalks.TileLayer(name, {
                            urlTemplate: 'http://api{s}.map.bdimg.com/customimage/tile?&x={x}&y={y}&z={z}&udt=20170406&scale=1&ak=8d6c8b8f3749aed6b1aff3aad6f40e37&styles=t%3Awater%7Ce%3Aall%7Cc%3A%23021019%2Ct%3Ahighway%7Ce%3Ag.f%7Cc%3A%23000000%2Ct%3Ahighway%7Ce%3Ag.s%7Cc%3A%23147a92%2Ct%3Aarterial%7Ce%3Ag.f%7Cc%3A%23000000%2Ct%3Aarterial%7Ce%3Ag.s%7Cc%3A%230b3d51%2Ct%3Alocal%7Ce%3Ag%7Cc%3A%23000000%2Ct%3Aland%7Ce%3Aall%7Cc%3A%2308304b%2Ct%3Arailway%7Ce%3Ag.f%7Cc%3A%23000000%2Ct%3Arailway%7Ce%3Ag.s%7Cc%3A%2308304b%2Ct%3Asubway%7Ce%3Ag%7Cl%3A-70%2Ct%3Abuilding%7Ce%3Ag.f%7Cc%3A%23000000%2Ct%3Aall%7Ce%3Al.t.f%7Cc%3A%23857f7f%2Ct%3Aall%7Ce%3Al.t.s%7Cc%3A%23000000%2Ct%3Abuilding%7Ce%3Ag%7Cc%3A%23022338%2Ct%3Agreen%7Ce%3Ag%7Cc%3A%23062032%2Ct%3Aboundary%7Ce%3Aall%7Cc%3A%231e1c1c%2Ct%3Amanmade%7Ce%3Aall%7Cc%3A%23022338',
                            subdomains: ['0', '1', '2']
                        });
                    case "BaiDu":
                    default:
                        return new maptalks.TileLayer(name, {
                            urlTemplate: 'http://online{s}.map.bdimg.com/tile/?qt=tile&x={x}&y={y}&z={z}&styles=pl&p=1',
                            // urlTemplate: 'http://api{s}.map.bdimg.com/customimage/tile?&x={x}&y={y}&z={z}&scale=1&customid=grayscale',
                            subdomains: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'] //不同服务器
                        });
                }
            };
            /**
             * 添加系统默认矢量图层
             */
            MapHelper.addSystemDefaultVectorLayer = function (map) {
                if (map) {
                    return longjin.layer.LayerHelper.createVectorLayer(longjin.core.constant._SYSTEM_DEFAULT_VECTOR_LAYER, map);
                }
                return null;
            };
            /**
             * 添加系统默认聚合图层
             */
            MapHelper.addSystemDefaultClusterLayer = function (map) {
                if (map) {
                    return longjin.layer.LayerHelper.createClusterLayer(longjin.core.constant._SYSTEM_DEFAULT_CLUSTER_LAYER, map);
                }
                return null;
            };
            /**
              添加系统默认热力图层
             */
            MapHelper.addSystemDefaultHeatLayer = function (map) {
                if (map) {
                    return longjin.layer.LayerHelper.createHeatLayer(longjin.core.constant._SYSTEM_DEFAULT_HEAT_LAYER, map);
                }
                return null;
            };
            /**
              添加系统默认光晕图层
             */
            MapHelper.addSystemDefaultHaloLayer = function (map) {
                if (map) {
                    return longjin.layer.LayerHelper.createHaloLayer(longjin.core.constant._SYSTEM_DEFAULT_HALO_LAYER, map);
                }
                return null;
            };
            /**
             * 添加系统特定功能图层
             */
            MapHelper.addSystemSpecialLayers = function (map) {
                var layers = [];
                layers.push(longjin.layer.LayerHelper.createClusterLayer(longjin.core.constant._SYSTEM_DEFAULT_CLUSTER_LAYER, map).hide());
                layers.push(longjin.layer.LayerHelper.createHeatLayer(longjin.core.constant._SYSTEM_DEFAULT_HEAT_LAYER, map).hide());
                layers.push(longjin.layer.LayerHelper.createHaloLayer(longjin.core.constant._SYSTEM_DEFAULT_HALO_LAYER, map).hide());
                //map.sortLayers([core.constant._SYSTEM_DEFAULT_CLUSTER_LAYER, core.constant._SYSTEM_DEFAULT_HEAT_LAYER, core.constant._SYSTEM_DEFAULT_VECTOR_LAYER]);
                return layers;
            };
            /**
             * 删除系统特定功能图层
             */
            MapHelper.removeSystemSpecialLayers = function (map) {
                if (map && map.getLayers) {
                    var _arr = [longjin.core.constant._SYSTEM_DEFAULT_CLUSTER_LAYER, longjin.core.constant._SYSTEM_DEFAULT_HEAT_LAYER, longjin.core.constant._SYSTEM_DEFAULT_HALO_LAYER];
                    var _layer_1;
                    /*$.each(_arr, function(idx, item) {
                        _layer = map.getLayer(item);
                        if(_layer) map.removeLayer(_layer);
                    });*/
                    _arr.forEach(function (item) {
                        _layer_1 = map.getLayer(item);
                        if (_layer_1)
                            map.removeLayer(_layer_1);
                    });
                }
            };
            /*
             * 从JSON中加载地图
             */
            MapHelper.fromJSON = function (container, mapJson, options) {
                /*$.extend(mapJson.options, options);
                mapJson.baseLayer = MapHelper.getTileLayer(mapJson.baseLayer.id, mapJson.options.type).toJSON();
                if(mapJson.options.type === "BaiDu") {
                    mapJson.options.view = {
                        projection: "baidu"
                    };
                } else {
                    mapJson.options.view = null;
                }*/
                var map = maptalks.Map.fromJSON(container, mapJson, options);
                MapHelper.addSystemSpecialLayers(map);
                return map;
            };
            /**
             * 导出JSON
             */
            MapHelper.toJSON = function (map) {
                if (!map)
                    return map;
                MapHelper.removeSystemSpecialLayers(map);
                var json = map.toJSON();
                /*if(json.layers && $.isArray(json.layers)) {
                    let defArr = [core.constant._SYSTEM_DEFAULT_CLUSTER_LAYER, core.constant._SYSTEM_DEFAULT_HEAT_LAYER];
                    $.each(json.layers, function(idx, item) {
                        if($.inArray(item.id, defArr) < 0) json.layers.splice(idx, 1);
                    });
                } else {
                    json.layers = [];
                }*/
                return json;
            };
            return MapHelper;
        }());
        map_1.MapHelper = MapHelper;
    })(map = longjin.map || (longjin.map = {}));
})(longjin || (longjin = {}));
/// <reference path="../jquery.d.ts" />
/// <reference path="../maptalks.d.ts" />
/// <reference path="../core/constant.ts" />
/// <reference path="../util/index.d.ts" />
/// <reference path="../layer/index.d.ts" />
/// <reference path="../geometry/index.d.ts" />
var longjin;
(function (longjin) {
    var control;
    (function (control) {
        /*扩展事件*/
        /*提交前*/
        //	const _EVENT_SUBMITING: string = "submitting";
        /*提交后*/
        //	const _EVENT_SUBMITED: string = "submitted";
        var ControlHelper = (function () {
            function ControlHelper() {
            }
            /*创建面板控件*/
            ControlHelper.createPanel = function (options) {
                return new maptalks.control.Panel(options);
            };
            /*创建搜索控件*/
            ControlHelper.createSearchBox = function (map, options) {
                var _panel = ControlHelper.createPanel(__assign({ position: {
                        top: 10,
                        left: 10
                    }, custom: true, content: "", closeButton: false, draggable: false }, options));
                _panel.setContent(ControlHelper._buildSearchBoxDom(map, _panel));
                if (map)
                    _panel.addTo(map);
                return _panel;
            };
            ControlHelper._buildSearchBoxDom = function (map, panel) {
                var $container = $("<div/>");
                $container.addClass("longjin-maptalks-searchbox");
                var $content = $("<input type='text' />").appendTo($container);
                $content.attr("id", "content");
                $content.attr("name", "content");
                $content.addClass("longjin-maptalks-searchbox-content");
                var $submit = $("<button />").appendTo($container);
                $submit.attr("id", "submit");
                $submit.attr("name", "submit");
                $submit.text("搜索");
                $submit.addClass("longjin-maptalks-searchbox-submit");
                $content.off("keyup").on("keyup", function (evt) {
                    if (evt.keyCode == 13) {
                        $submit.click();
                    }
                });
                $submit.off("click").on("click", function () {
                    var addr = $content.val();
                    panel.fire(longjin.core.constant._EVENT_SUBMITING, {
                        search: addr
                    });
                    longjin.util.CoordinateConverter.fromAddressToBDLngLatAsync(addr, function (data) {
                        // 坐标转化
                        //if(map.getType() != "BaiDu") data = util.CoordinateConverter.fromSourceToTarget(data, "BaiDu", map.getType());
                        //let _marker_layer = map.getLayer(core.constant._SYSTEM_DEFAULT_VECTOR_LAYER);
                        //if(!_marker_layer) {
                        //_marker_layer = layer.LayerHelper.createVectorLayer(core.constant._SYSTEM_DEFAULT_VECTOR_LAYER, map);
                        //}
                        //let geomarker = geometry.GeometryHelper.createMarker(data, _marker_layer);
                        //geomarker.off("click").on("click", function() {
                        // do something
                        //});
                        //geomarker.off("contextmenu").on("contextmenu", function() {
                        //return false;
                        //});
                        this.blur();
                        //if(map) map.panTo(data);
                        panel.fire(longjin.core.constant._EVENT_SUBMITED, {
                            search: addr,
                            coordinate: data
                        });
                    });
                });
                return $container[0];
            };
            /*static createLayerSelector(map: any, options: PanelOptions) {
                let _panel = ControlHelper.createPanel({
                    position: {
                        top: 10,
                        left: 10
                    },
                    custom: true,
                    content: "",
                    closeButton: false,
                    draggable: false,
                    ...options
                });
                _panel.setContent(ControlHelper._buildLayerSelectorDom(map, _panel));
                if(map) _panel.addTo(map)
                return _panel;
            }
    
            private static _buildLayerSelectorDom(map: any, panel: any): HTMLElement {
                let $container = $("<div/>");
                $container.addClass("longjin-maptalks-layerselector");
    
                <select id="layers" onchange="">
                    <option value="">全部</option>
                    <option value="1">a</option>
                    <option value="2">b</option>
                </select>
                
                let $select = $("<select />").appendTo($container);
                $select.attr("id", "longjin-maptalks-layers");
                $select.attr("name", "longjin-maptalks-layers");
                //$select.addClass("longjin-maptalks-layerselector-content");
    
                $select.off("change").on("change", function(evt: any) {
                    if(evt.keyCode == 13) {
                        $submit.click();
                    }
                });
    
                $submit.off("click").on("click", function() {
                    let addr: string = $content.val();
                    panel.fire(core.constant._EVENT_SUBMITING, {
                        search: addr
                    });
                    util.CoordinateConverter.fromAddressToBDLngLatAsync(addr, function(data) {
                        this.blur();
                        //if(map) map.panTo(data);
                        panel.fire(core.constant._EVENT_SUBMITED, {
                            search: addr,
                            coordinate: data
                        });
                    });
                });
                return $container[0];
            }*/
            /*
             * 创建工具条
             */
            ControlHelper.createToolbar = function (map, options) {
                var _toolbar = new maptalks.control.Toolbar(options);
                if (map)
                    _toolbar.addTo(map);
                return _toolbar;
            };
            ControlHelper.createZoom = function (map, options) {
                var _zoom = new maptalks.control.Zoom(__assign({ position: {
                        bottom: 10,
                        right: 10
                    }, slider: false, zoomLevel: true }, options));
                if (map)
                    _zoom.addTo(map);
                return _zoom;
            };
            ControlHelper.createScale = function (map, options) {
                var _scale = new maptalks.control.Scale(__assign({ position: {
                        bottom: 10,
                        left: 10
                    }, maxWidth: 100, metric: true, imperial: false }, options));
                if (map)
                    _scale.addTo(map);
                return _scale;
            };
            return ControlHelper;
        }());
        control.ControlHelper = ControlHelper;
    })(control = longjin.control || (longjin.control = {}));
})(longjin || (longjin = {}));
/// <reference path="../maptalks.d.ts" />
/// <reference path="../core/constant.ts" />
var longjin;
(function (longjin) {
    var layer;
    (function (layer) {
        /*图层帮助类*/
        var LayerHelper = (function () {
            function LayerHelper() {
            }
            LayerHelper.buildLayeyId = function (prefix) {
                prefix = prefix || longjin.core.constant._SYSTEM_DEFAULT_LAYER_ID_PREFIX;
                return prefix + longjin.util.Common.getCurrentTimestamp() + "_" + longjin.util.Common.randomString(4);
            };
            /*
             * 创建矢量图层
             */
            LayerHelper.createVectorLayer = function (id, map, options) {
                var _layer = null;
                if (map) {
                    _layer = map.getLayer(id);
                    if (_layer)
                        return _layer;
                }
                _layer = new maptalks.VectorLayer(id, options);
                if (map)
                    _layer.addTo(map);
                return _layer;
            };
            /*
             * 创建热力图层
             */
            LayerHelper.createHeatLayer = function (id, map, options) {
                var _layer = null;
                if (map) {
                    _layer = map.getLayer(id);
                    if (_layer)
                        return _layer;
                }
                _layer = new maptalks.HeatLayer(id, [], options);
                if (map)
                    _layer.addTo(map);
                return _layer;
            };
            /*
             * 创建聚合图层
             */
            LayerHelper.createClusterLayer = function (id, map, options) {
                var _layer = null;
                if (map) {
                    _layer = map.getLayer(id);
                    if (_layer)
                        return _layer;
                }
                _layer = new maptalks.ClusterLayer(id, {
                    symbol: [{
                            markerType: 'ellipse',
                            markerFill: {
                                type: 'radial',
                                colorStops: [
                                    [0.40, 'rgba(17, 172, 263, 1)'],
                                    [0.00, 'rgba(17, 172, 263, 0)'],
                                ]
                            },
                            markerFillOpacity: 0.6,
                            markerLineWidth: 0,
                            markerWidth: {
                                property: 'count',
                                type: 'interval',
                                stops: [
                                    [0, 60],
                                    [9, 80],
                                    [99, 120]
                                ]
                            },
                            markerHeight: {
                                property: 'count',
                                type: 'interval',
                                stops: [
                                    [0, 60],
                                    [9, 80],
                                    [99, 120]
                                ]
                            }
                        },
                        {
                            markerType: 'ellipse',
                            markerFill: '#fff',
                            markerFillOpacity: 1,
                            markerLineOpacity: 1,
                            markerLineWidth: 6,
                            markerLineColor: '#117ab3',
                            markerWidth: {
                                property: 'count',
                                type: 'interval',
                                stops: [
                                    [0, 40],
                                    [9, 60],
                                    [99, 80]
                                ]
                            },
                            markerHeight: {
                                property: 'count',
                                type: 'interval',
                                stops: [
                                    [0, 40],
                                    [9, 60],
                                    [99, 80]
                                ]
                            }
                        }
                    ],
                    textSymbol: {
                        textFaceName: '"microsoft yahei"',
                        textSize: 20,
                        textFill: '#117ab3'
                    },
                    //是否在cluster上显示元素数量
                    drawClusterText: true
                });
                if (map)
                    _layer.addTo(map);
                return _layer;
            };
            /*
             * 创建光晕图层
             */
            LayerHelper.createHaloLayer = function (id, map, options) {
                var _layer = null;
                if (map) {
                    _layer = map.getLayer(id);
                    if (_layer)
                        return _layer;
                }
                _layer = new maptalks.HaloLayer(id, [], __assign({ symbol: {
                        markerFill: {
                            property: 'mag',
                            type: 'interval',
                            stops: [
                                [0, LayerHelper.getGradient([135, 196, 240])],
                                [2, LayerHelper.getGradient([255, 255, 0])],
                                [5, LayerHelper.getGradient([216, 115, 149])]
                            ]
                        },
                        markerFillOpacity: 0.8,
                        markerWidth: {
                            property: 'mag',
                            type: 'interval',
                            stops: [
                                [0, 5],
                                [2, 12],
                                [5, 30]
                            ]
                        }
                    } }, options));
                if (map)
                    _layer.addTo(map);
                return _layer;
            };
            LayerHelper.getGradient = function (colors) {
                // return 'rgba(' + colors.join() + ', 0.5)'
                return {
                    type: 'radial',
                    colorStops: [
                        [0.70, 'rgba(' + colors.join() + ', 0.5)'],
                        [0.30, 'rgba(' + colors.join() + ', 1)'],
                        [0.20, 'rgba(' + colors.join() + ', 1)'],
                        [0.00, 'rgba(' + colors.join() + ', 0)']
                    ]
                };
            };
            /*
             * 将Json反序列化成图层
             */
            LayerHelper.fromJSON = function (map, layerJson) {
                var _layer = maptalks.Layer.fromJSON(layerJson);
                if (map)
                    _layer.addTo(map);
                return _layer;
            };
            return LayerHelper;
        }());
        layer.LayerHelper = LayerHelper;
    })(layer = longjin.layer || (longjin.layer = {}));
})(longjin || (longjin = {}));
/// <reference path="../jquery.d.ts" />
/// <reference path="../maptalks.d.ts" />
/// <reference path="../util/index.d.ts" />
/// <reference path="../core/constant.ts" />
var longjin;
(function (longjin) {
    var geometry;
    (function (geometry_1) {
        /*几何图形帮助类*/
        var GeometryHelper = (function () {
            function GeometryHelper() {
            }
            /**
             * 生成图形ID
             */
            GeometryHelper.buildGeometryId = function (prefix) {
                prefix = prefix || longjin.core.constant._SYSTEM_DEFAULT_GEOMETRY_ID_PREFIX;
                return prefix + longjin.util.Common.getCurrentTimestamp() + "_" + longjin.util.Common.randomString(4);
            };
            GeometryHelper.toGeometry = function (marker) {
                if (!marker)
                    return null;
                return maptalks.GeoJSON.toGeometry(marker.toJSON().feature);
            };
            /*创建几何标注*/
            GeometryHelper.createMarker = function (coordinate, layer, options) {
                if (!coordinate)
                    return null;
                var _id = GeometryHelper.buildGeometryId();
                var _marker = null;
                if (layer) {
                    _marker = layer.getGeometryById(_id);
                    if (_marker)
                        return _marker;
                }
                _marker = new maptalks.Marker(coordinate, __assign({ id: _id, symbol: {
                        'markerType': 'path',
                        'markerPath': 'M8 23l0 0 0 0 0 0 0 0 0 0c-4,-5 -8,-10 -8,-14 0,-5 4,-9 8,-9l0 0 0 0c4,0 8,4 8,9 0,4 -4,9 -8,14z M5,9 a3,3 0,1,0,0,-0.9Z',
                        'markerPathWidth': 16,
                        'markerPathHeight': 23,
                        'markerWidth': 24,
                        'markerHeight': 34,
                        'markerFill': 'Red'
                    } }, options));
                if (layer) {
                    _marker.addTo(layer);
                }
                return _marker;
            };
            /*
             * 创建多边形
             */
            GeometryHelper.createPolygon = function (coordinates, layer, options) {
                var _polygon = new maptalks.Polygon(coordinates, {
                    id: GeometryHelper.buildGeometryId(),
                    symbol: {
                        lineColor: '#34495e',
                        lineWidth: 2,
                        polygonFill: 'rgb(135,196,240)',
                        polygonOpacity: 0.6
                    }
                });
                if (layer)
                    _polygon.setZIndex(500).addTo(layer);
                return _polygon;
            };
            /*
             * 根据条件设置几何图形的样式
             */
            GeometryHelper.setSymbolByCondition = function (geometry, value, conditions) {
                if (!geometry || !conditions)
                    return;
                if (typeof value == "string" && $.trim(value).length <= 0)
                    return;
                var _symbol_cb = longjin.util.Common.evalConditions(value, conditions);
                if (_symbol_cb) {
                    if (_symbol_cb.symbol)
                        geometry.setSymbol(_symbol_cb.symbol);
                    if (_symbol_cb.callback && $.isFunction(_symbol_cb.callback))
                        _symbol_cb.callback({
                            geometry: geometry,
                            value: value,
                            symbol: _symbol_cb.symbol
                        });
                }
            };
            /**
             * 计算几何图形内点的数量
             */
            GeometryHelper.computePointCountOfGeometry = function (geometry, filter) {
                if (geometry && geometry.getLayer()) {
                    var layer_1 = geometry.getLayer();
                    var points = layer_1.getGeometries(function (item) {
                        if (item.getType() == "Point")
                            return item;
                    });
                    if (filter && $.isFunction(filter) && points.length > 0) {
                        var _arr = new Array();
                        for (var _i = 0, points_1 = points; _i < points_1.length; _i++) {
                            var p = points_1[_i];
                            if (filter({
                                container: geometry,
                                point: p
                            }))
                                _arr.push(p);
                        }
                        return _arr.length;
                    }
                    return points.length;
                }
                return 0;
            };
            /**
             * 点落在某个区域,改变区域样式
             */
            GeometryHelper.setGeometrySymbolIfContainsPoint = function (coordinate, geometry, symbol) {
                if (!coordinate || !geometry)
                    return;
                if (symbol && geometry.containsPoint(coordinate)) {
                    geometry.setSymbol(symbol);
                }
            };
            /**
             * 设置包含指定坐标的所有图形样式
             */
            GeometryHelper.setSymbolIfContainsPoint = function (coordinate, layer, symbol) {
                if (!coordinate || !layer)
                    return;
                var geometries = layer.getGeometries();
                if (geometries && $.isArray(geometries)) {
                    for (var _i = 0, geometries_1 = geometries; _i < geometries_1.length; _i++) {
                        var g = geometries_1[_i];
                        if (g.getType() != "Point")
                            GeometryHelper.setGeometrySymbolIfContainsPoint(coordinate, g, symbol);
                    }
                }
            };
            /**
             * 当满足条件时触发警报
             */
            GeometryHelper.executeWhenSatisfied = function (filter, filterContext, action, actionContext) {
                if (filter && (filterContext ? filter(filterContext) : filter()) && action) {
                    actionContext ? action(actionContext) : action();
                }
            };
            return GeometryHelper;
        }());
        geometry_1.GeometryHelper = GeometryHelper;
    })(geometry = longjin.geometry || (longjin.geometry = {}));
})(longjin || (longjin = {}));
/// <reference path="../jquery.d.ts" />
/// <reference path="../maptalks.d.ts" />
/// <reference path="../geometry/index.d.ts" />
var longjin;
(function (longjin) {
    var animation;
    (function (animation) {
        /*动画帮助类*/
        var AnimationHelper = (function () {
            function AnimationHelper() {
            }
            /*
             * 添加光晕效果
             */
            /*static addHalo(coordinates: Array < [number, number] > , map: any, options ? : geometry.GeometryMarkerOptions) {
                if(!coordinates || !map) return;
                let _layer = map.getLayer(core.constant._SYSTEM_DEFAULT_HALO_LAYER);
                if(!_layer) {
                    _layer = layer.LayerHelper.createHaloLayer(core.constant._SYSTEM_DEFAULT_HALO_LAYER);
                } else {
                    map.removeLayer(_layer);
                }
                let _geos = _layer.getGeometries();
                if(coordinates.length > 0) {
                    for(let coordinate of coordinates) {
                        _geos.push(geometry.GeometryHelper.toGeometry(geometry.GeometryHelper.createMarker(coordinate, null, { ...options, id: util.Common.coordinateToId(coordinate, "_HALO_") })));
                    }
                }
                _layer.clear().addGeometry(_geos);
                map.addLayer(_layer);
                // if(!_layer.isVisible()) _layer.show();
                map.sortLayers([core.constant._SYSTEM_DEFAULT_CLUSTER_LAYER, core.constant._SYSTEM_DEFAULT_HEAT_LAYER, core.constant._SYSTEM_DEFAULT_HALO_LAYER, core.constant._SYSTEM_DEFAULT_VECTOR_LAYER]);
            }*/
            AnimationHelper.addHalo = function (coordinates, layer, options) {
                if (!coordinates || coordinates.length <= 0 || !layer)
                    return;
                var _geos = layer.getGeometries();
                if (coordinates.length > 0) {
                    for (var _i = 0, coordinates_5 = coordinates; _i < coordinates_5.length; _i++) {
                        var coordinate = coordinates_5[_i];
                        _geos.push(longjin.geometry.GeometryHelper.toGeometry(longjin.geometry.GeometryHelper.createMarker(coordinate, null, __assign({ id: longjin.util.Common.coordinateToId(coordinate, "_HALO_") }, options))));
                    }
                }
                layer.clear().addGeometry(_geos);
            };
            /*
             * 移除光晕效果
             */
            /*static removeHalo(coordinates: Array < [number, number] > , map: any) {
                if(!coordinates || !map) return;
                let _layer = map.getLayer(core.constant._SYSTEM_DEFAULT_HALO_LAYER);
                if(!_layer) return;
                map.removeLayer(_layer);
                if(coordinates.length > 0) {
                    let _geometry = null;
                    for(let coordinate of coordinates) {
                        _geometry = _layer.getGeometryById(util.Common.coordinateToId(coordinate, "_HALO_"));
                        if(_geometry) _layer.removeGeometry(_geometry);
                    }
                }
                map.addLayer(_layer);
            }*/
            AnimationHelper.removeHalo = function (coordinates, layer) {
                if (!coordinates || !layer)
                    return;
                if (coordinates.length > 0) {
                    var _geometry = null;
                    for (var _i = 0, coordinates_6 = coordinates; _i < coordinates_6.length; _i++) {
                        var coordinate = coordinates_6[_i];
                        _geometry = layer.getGeometryById(longjin.util.Common.coordinateToId(coordinate, "_HALO_"));
                        if (_geometry)
                            layer.removeGeometry(_geometry);
                    }
                }
            };
            return AnimationHelper;
        }());
        animation.AnimationHelper = AnimationHelper;
    })(animation = longjin.animation || (longjin.animation = {}));
})(longjin || (longjin = {}));
/// <reference path="../jquery.d.ts" />
/// <reference path="../maptalks.d.ts" />
var longjin;
(function (longjin) {
    var geo;
    (function (geo) {
        /*地理帮助类*/
        var GeoHelper = (function () {
            function GeoHelper() {
            }
            return GeoHelper;
        }());
        geo.GeoHelper = GeoHelper;
    })(geo = longjin.geo || (longjin.geo = {}));
})(longjin || (longjin = {}));
/// <reference path="../jquery.d.ts" />
/// <reference path="../maptalks.d.ts" />
var longjin;
(function (longjin) {
    var ui;
    (function (ui) {
        /*UI组件帮助类*/
        var UIHelper = (function () {
            function UIHelper() {
            }
            /*创建一个UI标记*/
            UIHelper.createMarker = function (coordinate, options) {
                /*let _default: UIMarkerOptions = {
                    draggable: true,
                    single: false,
                    content: ""
                };*/
                /*$.extend(true, _default, options);*/
                /*return new maptalks.ui.UIMarker([coordinate.x, coordinate.y], _default);*/
                return new maptalks.ui.UIMarker([coordinate[0], coordinate[1]], __assign({ draggable: true, single: false, content: "" }, options));
            };
            UIHelper.createInfoWindow = function (map, coordinate, options) {
                if (!map)
                    return null;
                var _wind = new maptalks.ui.InfoWindow(__assign({ title: "标题", content: "内容" }, options)).addTo(map);
                if (coordinate)
                    return _wind.show(coordinate);
                return _wind;
            };
            return UIHelper;
        }());
        ui.UIHelper = UIHelper;
    })(ui = longjin.ui || (longjin.ui = {}));
})(longjin || (longjin = {}));
/// <reference path="../maptalks.d.ts" />
var longjin;
(function (longjin) {
    var tool;
    (function (tool) {
        var ToolHelper = (function () {
            function ToolHelper() {
            }
            /*
             * 创建自定义工具
             */
            ToolHelper.createTool = function (map, options) {
                var CustomTool = maptalks.MapTool.extend(__assign({ initialize: function () { },
                    onEnable: function () { },
                    getEvents: function () {
                        return {};
                    },
                    onDisable: function () { } }, options));
                var _tool = new CustomTool();
                if (map)
                    _tool.addTo(map);
                return _tool;
            };
            /*
             * 创建测距工具
             */
            ToolHelper.createDistanceTool = function (map, options) {
                var _tool = new maptalks.DistanceTool(__assign({ 'symbol': {
                        'lineColor': '#34495e',
                        'lineWidth': 2
                    }, 'vertexSymbol': {
                        'markerType': 'ellipse',
                        'markerFill': '#1bbc9b',
                        'markerLineColor': '#000',
                        'markerLineWidth': 3,
                        'markerWidth': 10,
                        'markerHeight': 10
                    }, 'language': 'en-US' }, options));
                if (map)
                    _tool.addTo(map);
                return _tool;
            };
            /*
             * 创建测面工具
             */
            ToolHelper.createAreaTool = function (map, options) {
                var _tool = new maptalks.AreaTool(__assign({ 'symbol': {
                        'lineColor': '#1bbc9b',
                        'lineWidth': 2,
                        'polygonFill': '#fff',
                        'polygonOpacity': 0.3
                    }, 'vertexSymbol': {
                        'markerType': 'ellipse',
                        'markerFill': '#34495e',
                        'markerLineColor': '#1bbc9b',
                        'markerLineWidth': 3,
                        'markerWidth': 10,
                        'markerHeight': 10
                    }, language: '' }, options));
                if (map)
                    _tool.addTo(map);
                return _tool;
            };
            /*
             * 创建画图形工具
             */
            ToolHelper.createDrawTool = function (map, options) {
                var _tool = new maptalks.DrawTool(__assign({ mode: 'Point' }, options));
                if (map)
                    _tool.addTo(map);
                return _tool;
            };
            return ToolHelper;
        }());
        tool.ToolHelper = ToolHelper;
    })(tool = longjin.tool || (longjin.tool = {}));
})(longjin || (longjin = {}));
