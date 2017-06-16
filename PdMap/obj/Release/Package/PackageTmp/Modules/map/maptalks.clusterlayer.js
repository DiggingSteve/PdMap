﻿(function () {
	'use strict';

	var maptalks;

	var nodeEnv = typeof module !== 'undefined' && module.exports;
	if (nodeEnv) {
		maptalks = require('maptalks');
	} else {
		maptalks = window.maptalks;
	}
	//
	function getGradient(color) {
		return 'rgba(' + color.join() + ', 1)';
		/*return {
            type : 'radial',
            colorStops : [
                [0.00, 'rgba(' + color.join() + ', 0)'],
                [0.50, 'rgba(' + color.join() + ', 1)'],
                [1.00, 'rgba(' + color.join() + ', 1)']
            ]
        };*/
	}

	var textSymbol = {
		'textFaceName': '"microsoft yahei"',
		'textSize': 16
	};

	var symbol = {
		'markerFill': { property: 'count', type: 'interval', stops: [[0, getGradient([135, 196, 240])/*getGradient([255, 226, 140])*/], [9, '#1bbc9b'/*getGradient([241, 211, 87])*/], [99, getGradient([216, 115, 149])]] },
		'markerFillOpacity': 0.7,
		'markerLineOpacity': 1,
		'markerLineWidth': 3,
		'markerLineColor': '#fff',
		'markerWidth': { property: 'count', type: 'interval', stops: [[0, 20], [9, 30], [99, 40]] },
		'markerHeight': 40
	};

	maptalks.ClusterLayer = maptalks.VectorLayer.extend({
		options: {
			'maxClusterRadius': 80,
			'geometryEvents': false,
			'symbol': symbol,
			'animation': true,
			'animationDuration': 450
		},

		addMarker: function (markers) {
			return this.addGeometry(markers);
		},

		addGeometry: function (markers) {
			for (var i = 0, len = markers.length; i <= len; i++) {
				if (!markers[i] instanceof maptalks.Marker) {
					throw new Error('Only a point(Marker) can be added into a ClusterLayer');
				}
			}
			return maptalks.VectorLayer.prototype.addGeometry.apply(this, arguments);
		}

	});

	/**
     * Export the ClusterLayer's profile JSON.
     * @return {Object} layer's profile JSON
     */
	maptalks.ClusterLayer.prototype.toJSON = function () {
		var json = maptalks.VectorLayer.prototype.toJSON.call(this);
		json['type'] = 'ClusterLayer';
		return json;
	};

	/**
     * Reproduce a ClusterLayer from layer's profile JSON.
     * @param  {Object} json - layer's profile JSON
     * @return {maptalks.ClusterLayer}
     * @static
     * @private
     * @function
     */
	maptalks.ClusterLayer.fromJSON = function (json) {
		if (!json || json['type'] !== 'ClusterLayer') { return null; }
		var layer = new maptalks.ClusterLayer(json['id'], json['options']);
		var geoJSONs = json['geometries'];
		var geometries = [],
            geo;
		for (var i = 0; i < geoJSONs.length; i++) {
			geo = maptalks.Geometry.fromJSON(geoJSONs[i]);
			if (geo) {
				geometries.push(geo);
			}
		}
		layer.addGeometry(geometries);
		return layer;
	};

	maptalks.renderer.clusterlayer = {};

	maptalks.renderer.clusterlayer.Canvas = maptalks.renderer.Canvas.extend({

		initialize: function (layer) {
			this.layer = layer;
			var symbolizer = maptalks.symbolizer.VectorMarkerSymbolizer;
			var style = symbolizer.translateLineAndFill(symbol);
			var argFn = maptalks.Util.bind(function () {
				return [this.getMap().getZoom(), this._currentGrid];
			}, this);
			this._style = maptalks.Util.loadFunctionTypes(style, argFn);
			this._symbol = maptalks.Util.loadFunctionTypes(symbol, argFn);
			var id = maptalks.internalLayerPrefix + '_grid_' + maptalks.Util.GUID();
			this._markerLayer = new maptalks.VectorLayer(id).addTo(layer.getMap());
			this._animated = true;
			this._computeGrid();
		},

		draw: function () {
			if (!this.canvas) {
				this.prepareCanvas();
			}
			var font = maptalks.symbolizer.TextMarkerSymbolizer.getFont(textSymbol);
			var map = this.getMap(),
                size = map.getSize(),
                extent = new maptalks.PointExtent(0, 0, size['width'], size['height']),
                symbol = this._symbol,
                marker, markers = [], clusters = [],
                pt, pExt, width;
			for (var p in this._grid) {
				this._currentGrid = this._grid[p];
				if (this._grid[p]['count'] === 1) {
					marker = this._grid[p]['geo'].copy().copyEventListeners(this._grid[p]['geo']);
					marker._cluster = this._grid[p];
					markers.push(marker);
					continue;
				}
				width = symbol['markerWidth'];
				pt = map._prjToContainerPoint(this._grid[p]['center']);
				pExt = new maptalks.PointExtent(pt.substract(width, width), pt.add(width, width));
				if (!extent.intersects(pExt)) {
					continue;
				}
				if (!this._grid[p]['size']) {
					this._grid[p]['size'] = maptalks.StringUtil.stringLength(this._grid[p]['count'], font).toPoint()._multi(1 / 2);
				}
				clusters.push(this._grid[p]);
			}
			this._drawLayer(clusters, markers);
		},

		show: function () {
			this._markerLayer.show();
			maptalks.renderer.Canvas.prototype.show.call(this);
		},

		hide: function () {
			this._markerLayer.hide();
			maptalks.renderer.Canvas.prototype.hide.call(this);
		},

		setZIndex: function (z) {
			this._markerLayer.setZIndex(z);
			maptalks.renderer.Canvas.prototype.setZIndex.call(this, z);
		},

		transform: function (matrix) {
			if (this._currentClusters) {
				this._drawClusters(this._currentClusters, 1, matrix);
			}
			return true;
		},

		_drawLayer: function (clusters, markers, matrix) {
			if (this._currentClusters && clusters.length === this._currentClusters.length) {
				this._animated = false;
			}
			this._currentClusters = clusters;
			var layer = this.layer;
			var me = this;
			if (layer.options['animation'] && this._animated && this._inout === 'out') {
				this._player = maptalks.Animation.animate(
                    { 'd': [0, 1] },
                    { 'speed': layer.options['animationDuration'], 'easing': 'inAndOut' },
                    function (frame) {
                    	if (frame.state.playState === 'finished') {
                    		if (!matrix && me._markerLayer.getCount() === 0) {
                    			me._markerLayer.addGeometry(markers);
                    		}
                    		me._animated = false;
                    		me.completeRender();
                    	} else {
                    		me._drawClusters(clusters, frame.styles.d, matrix);
                    		me.requestMapToRender();
                    	}
                    }
                )
                .play();
				this._drawClusters(clusters, 0, matrix);
				this.requestMapToRender();
			} else {
				this._drawClusters(clusters, 1, matrix);
				if (!matrix && this._markerLayer.getCount() === 0) {
					this._markerLayer.addGeometry(markers);
				}
				this.completeRender();
			}
		},

		_drawClusters: function (clusters, ratio, matrix) {
			matrix = matrix ? matrix['container'] : null;
			this.prepareCanvas();
			var map = this.getMap(),
                ctx = this.context,
                drawn = {};
			maptalks.Canvas.prepareCanvasFont(ctx, textSymbol);
			clusters.forEach(function (c) {
				if (c.parent) {
					var parent = map._prjToContainerPoint(c.parent['center']);
					if (!drawn[c.parent.key]) {
						if (matrix) {
							parent = matrix.applyToPointInstance(parent);
						}
						drawn[c.parent.key] = 1;
						this._drawCluster(parent, c.parent, 1 - ratio);
					}
				}
			}, this);
			if (ratio === 0) {
				return;
			}
			clusters.forEach(function (c) {
				var pt = map._prjToContainerPoint(c['center']);
				if (c.parent) {
					var parent = map._prjToContainerPoint(c.parent['center']);
					pt = parent.add(pt.substract(parent)._multi(ratio));
				}
				if (matrix) {
					pt = matrix.applyToPointInstance(pt);
				}
				this._drawCluster(pt, c, ratio > 0.5 ? 1 : ratio);
			}, this);

		},

		_drawCluster: function (pt, grid, op) {
			this._currentGrid = grid;
			var ctx = this.context,
                symbol = this._symbol,
                width = symbol['markerWidth'];
			var pExt = new maptalks.PointExtent(pt.substract(width, width), pt.add(width, width));
			this._style['polygonGradientExtent'] = pExt;
			maptalks.Canvas.prepareCanvas(ctx, this._style);
			ctx.beginPath();
			ctx.arc(pt.x, pt.y, width, 0, 2 * Math.PI);
			maptalks.Canvas._stroke(ctx, symbol['markerLineOpacity'] * op);
			maptalks.Canvas.fillCanvas(ctx, symbol['markerFillOpacity'] * op);
			if (grid['size']) {
				maptalks.Canvas.fillText(ctx, grid['count'], pt.substract(grid['size']), 'rgba(0,0,0,' + op + ')');
			}
		},

		_computeGrid: function () {
			var map = this.getMap(),
                zoom = map.getZoom();
			if (!this._gridCache) {
				this._gridCache = {};
			}
			var pre = map._getResolution(map.getMinZoom()) > map._getResolution(map.getMaxZoom()) ? zoom - 1 : zoom + 1;
			if (this._gridCache[pre] && this._gridCache[pre].length === this.layer.getCount()) {
				this._gridCache[zoom] = this._gridCache[pre];
			}
			if (!this._gridCache[zoom]) {
				this._gridCache[zoom] = this._computeZoomGrid(zoom);
			}
			this._grid = this._gridCache[zoom];
		},

		_computeZoomGrid: function (zoom) {
			var map = this.getMap(),
                t = map._getResolution(zoom) * this.layer.options['maxClusterRadius'] * 2,
                preCache = this._gridCache[zoom - 1],
                preT = map._getResolution(zoom - 1) ? map._getResolution(zoom - 1) * this.layer.options['maxClusterRadius'] * 2 : null;
			if (!preCache && zoom - 1 >= map.getMinZoom()) {
				this._gridCache[zoom - 1] = preCache = this._computeZoomGrid(zoom - 1);
			}
			var extent = this._markerExtent, points = this._markerPoints;
			if (!extent || !points) {
				if (!points) {
					points = [];
				}
				var c;
				this.layer.forEach(function (g) {
					c = g._getPrjCoordinates();
					if (!extent) {
						extent = g._getPrjExtent();
					} else {
						extent = extent._combine(g._getPrjExtent());
					}
					points.push({
						x: c.x,
						y: c.y,
						id: g._getInternalId(),
						geometry: g
					});
				}, this);
				this._markerExtent = extent;
				this._markerPoints = points;
			}
			if (!extent) {
				return null;
			}
			var grid = {},
                min = extent.getMin(),
                gx, gy, key,
                pgx, pgy, pkey;
			for (var i = 0, len = points.length; i < len; i++) {
				gx = Math.floor((points[i].x - min.x) / t);
				gy = Math.floor((points[i].y - min.y) / t);
				key = gx + '_' + gy;
				if (!grid[key]) {
					grid[key] = {
						'sum': new maptalks.Coordinate(points[i].x, points[i].y),
						'center': new maptalks.Coordinate(points[i].x, points[i].y),
						'count': 1,
						'geo': points[i].geometry,
						'key': key + ''
					};
					if (preT && preCache) {
						pgx = Math.floor((points[i].x - min.x) / preT);
						pgy = Math.floor((points[i].y - min.y) / preT);
						pkey = pgx + '_' + pgy;
						grid[key]['parent'] = preCache[pkey];
					}
				} else {
					grid[key]['sum']._add(new maptalks.Coordinate(points[i].x, points[i].y));
					grid[key]['count']++;
					grid[key]['center'] = grid[key]['sum'].multi(1 / grid[key]['count']);

				}
			}

			return grid;
		},

		onZoomStart: function (param) {
			this._inout = param['from'] > param['to'] ? 'in' : 'out';
			if (this._markerLayer.getCount() > 0) {
				this._markerLayer.clear();
			}
			if (this._player && this._player.playState !== 'finished') {
				this._player.finish();
			}
		},

		onZoomEnd: function () {
			this._animated = true;
			this._computeGrid();
			maptalks.renderer.Canvas.prototype.onZoomEnd.apply(this, arguments);
		}
	});

	maptalks.ClusterLayer.registerRenderer('canvas', maptalks.renderer.clusterlayer.Canvas);

	if (nodeEnv) {
		exports = module.exports = maptalks.ClusterLayer;
	}
})();