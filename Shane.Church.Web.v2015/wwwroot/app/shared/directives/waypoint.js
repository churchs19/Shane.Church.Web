angular.module('shane.church.shared.waypoint', [])
	.controller('waypointCtrl' ['$scope', function($scope) {

	}])
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