angular.module('shane.church.shared.flickity', [])
	.directive('flickity', [function () {
		function link(scope, element, attrs) {
			var $attrs = attrs;
			var $element = $(element);
			scope.$on('contentReady', function () {
//				attrs.$observe('flickity', function (val) {
				//					var options = angular.fromJson(val);
				var options = angular.fromJson($attrs.flickity);
					$element.flickity(options);
//				});
			});
		}

		return {
			link: link
		};
	}]);