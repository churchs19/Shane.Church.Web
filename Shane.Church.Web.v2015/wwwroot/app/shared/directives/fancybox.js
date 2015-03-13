angular.module('shane.church.shared.fancybox', [])
	.directive('fancybox', [function () {
		function link(scope, element, attrs) {
			attrs.$observe('fancybox', function (val) {
				var options = angular.fromJson(val);
				$(element).fancybox(options);
			});
		}

		return {
			link: link
		};
	}]);