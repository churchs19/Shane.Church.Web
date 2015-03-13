angular.module('shane.church.shared.flickity', [])
	.directive('flickity', [function () {
		function link(scope, element, attrs) {
			attrs.$observe('flickity', function (val) {
				var options = angular.fromJson(val);
				$(element).flickity(options);
			});
		}

		return {
			link: link
		};
	}]);