angular.module('shane.church.shared.photoItem', [])
.directive('photoItem', function () {
	return {
		restrict: 'E',
		scope: {
			photoData: '=photo'
		},
		templateUrl: 'app/shared/directives/photo-item.html'
	};
});