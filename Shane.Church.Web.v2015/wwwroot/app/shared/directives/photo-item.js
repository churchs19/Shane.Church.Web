angular.module('shane.church.shared', [])
.directive('photo-item', function () {
	return {
		restrict: 'E',
		scope: {
			photo: '=photo'
		},
		templateUrl: 'photo-item.html'
	};
});