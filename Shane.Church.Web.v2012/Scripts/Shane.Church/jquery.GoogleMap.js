//jquery.GoogleMap.js
//Copyright © Shane Church 2011

(function ($) {
	google.maps.Marker.prototype.pageId = -1; //Adds the pageId property to the Google Maps Marker object prototype

	var methods = {
		init: function (options) {
			//Initializes the Google Map for the specified selector
			var defaults = {
				latitude: 39.75517,
				longitude: -104.99099089999998,
				callback: function (map) { },
				panControlVisible: true,
				zoomControlVisible: true,
				mapTypeControlVisible: true,
				scaleControlVisible: true,
				streetViewControlVisible: true,
				overviewMapControlVisible: true,
				zoom: 4,
				mapType: google.maps.MapTypeId.ROADMAP
			};

			var opts = $.extend(defaults, options);

			var $this = $(this);

			return $this.each(function () {
				var $map = $(this);

				var latlng = new google.maps.LatLng(opts.latitude, opts.longitude);
				//Create the map options object
				var myOptions = {
					zoom: opts.zoom,
					center: latlng,
					mapTypeControl: opts.mapTypeControlVisible,
					panControl: opts.panControlVisible,
					zoomControl: opts.zoomControlVisible,
					scaleControl: opts.scaleControlVisible,
					streetViewControl: opts.streetViewControlVisible,
					overviewMapControl: opts.overviewMapControlVisible,
					mapTypeId: opts.mapType
				};
				//Create the map with the specified options
				var map = new google.maps.Map(this, myOptions);

				$map.data("map_object", map);  //Use the jQuery data function to store the map object with the incoming element
				$map.data("markers_array", []);

				opts.callback(map);
			});
		},
		addMarker: function (options) {
			//Adds a marker to the specified map
			var defaults = {
				id: '',
				latitude: 39.75517,
				longitude: -104.99099089999998,
				description: "Default Pin",
				infoWindowHtml: "",
				iconImageUrl: "",
				zIndex: 10,
				callback: function (map) { },
				infoboxDrawCallback: function (box) { }
			};

			var opts = $.extend(defaults, options);

			var $this = $(this);

			return $this.each(function () {
				var $map = $(this);
				var map = $map.data("map_object");
				var markersArray = $map.data("markers_array");

				var latlng = new google.maps.LatLng(opts.latitude, opts.longitude);

				var markerOptions = {
					position: latlng,
					map: map,
					title: opts.description,
					zIndex: opts.zIndex
				};

				if (opts.iconImageUrl !="" && opts.iconImageUrl != undefined) {
					var iconImage = new google.maps.MarkerImage(opts.iconImageUrl,
						new google.maps.Size(36, 35),
						// The origin for this image is 0,0.
						new google.maps.Point(0,0),
						// The anchor for this image is the base of the flagpole at 0,32.
						new google.maps.Point(16, 16));
					markerOptions.icon = iconImage;
				}

				var marker = new google.maps.Marker(markerOptions);
				marker.pageId = opts.id;

				if (opts.infoWindowHtml != "" && opts.infoWindowHtml != undefined) {
					var infoBoxOptions = {
						content: opts.infoWindowHtml,
						disableAutoPan: false,
						maxWidth: 0,
						pixelOffset: new google.maps.Size(-152, -265),
						zIndex: null,
						boxClass: "infoBubble",
						closeBoxMargin: "10px 10px 10px 10px",
						infoBoxClearance: new google.maps.Size(60, 60),
						enableEventPropagation: false
					 };

					var infobox = new InfoBox(infoBoxOptions);
					infobox.drawCallback = opts.infoboxDrawCallback;

					google.maps.event.addListener(marker, 'click', function () {
						infobox.open(map, marker);
					});					
				}

				markersArray.push(marker);

				$map.data("markers_array", markersArray);

				opts.callback(map);
			});
		},
		zoomAll: function (options) {
			//This function sets the map's boundaries to include all markers in the boundaries
			var defaults = {
				callback: function (map) { }
			};

			var opts = $.extend(defaults, options);

			var $this = $(this);

			return $this.each(function () {
				var $map = $(this);
				var map = $map.data("map_object");
				var markersArray = $map.data("markers_array");
				var bounds = new google.maps.LatLngBounds();

				for (var i = 0; i < markersArray.length; i++) {
					bounds.extend(markersArray[i].getPosition());
				}

				if (!bounds.isEmpty()) {
					map.fitBounds(bounds);
				}

				opts.callback(map);
			});
		},
		getMap: function () {
			//Returns the Google Maps object contained in the specified selector
			return $(this).data("map_object");
		},
		hideMarker: function (pageId) {
			//Hides the marker specified by the input Cartegraph id
			var $map = $(this);
			var map = $map.data("map_object");
			var markersArray = $map.data("markers_array");

			for (var i = 0; i < markersArray.length; i++) {
				if (markersArray[i].pageId == pageId) {
					markersArray[i].setVisible(false);
					return;
				}
			}
		},
		showMarker: function (pageId) {
			//Shows the marker specified by the input Cartegraph id
			var $map = $(this);
			var map = $map.data("map_object");
			var markersArray = $map.data("markers_array");

			for (var i = 0; i < markersArray.length; i++) {
				if (markersArray[i].pageId == pageId) {
					markersArray[i].setVisible(true);
					return;
				}
			}
		},
		hideAllMarkersExceptList: function (pageIds) {
			//Hides all markers except those in the input array of ids
			var $map = $(this);
			var map = $map.data("map_object");
			var markersArray = $map.data("markers_array");
			var visibleArray = [];

			if (markersArray != undefined) {
				for (var k = 0; k < markersArray.length; k++) {
					visibleArray.push(false);
					for (var j = 0; j < pageIds.length; j++) {
						if (markersArray[k].pageId == pageIds[j] && !visibleArray[k]) {
							visibleArray[k] = true;
							break;
						}
					}
				}
				for (var i = 0; i < visibleArray.length; i++) {
					markersArray[i].setVisible(visibleArray[i]);
				}
			}
		},
		resize: function () {
			var $map = $(this);
			var map = $map.data("map_object");
			google.maps.event.trigger(map, 'resize');			
		}	,
		panToLocation: function (options) {
			var defaults = {
				latitude: 39.75517,
				longitude: -104.99099089999998,
				callback: function (map) { }
			};

			var opts = $.extend(defaults, options);

			var $this = $(this);

			return $this.each(function () {
				var $map = $(this);
				var map = $map.data("map_object");

				var latlng = new google.maps.LatLng(opts.latitude, opts.longitude);

				map.panTo(latlng);

				opts.callback(map);
			});
		}
	};

	//jQuery Google Map Method initializer
	$.fn.GoogleMap = function (method) {
		if (methods[method]) {
			return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return methods.init.apply(this, arguments);
		} else {
			$.error('Method ' + method + ' does not exist on jQuery.GoogleMap');
		}
	};

})(jQuery);
