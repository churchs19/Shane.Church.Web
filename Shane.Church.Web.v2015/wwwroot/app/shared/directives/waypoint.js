﻿angular.module('shane.church.shared.waypoint', [])
	.directive('waypoint', [function () {
		function link(scope, element, attrs) {			
			$(element).waypoint(function() {
				$(element).addClass(attrs.waypointClasses);
			}, {
				offset: attrs.waypointOffset
			});
		}

		return {
			link: link
		};
	}]);