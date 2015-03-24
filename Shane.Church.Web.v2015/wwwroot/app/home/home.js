'use strict';

angular.module('shane.church.home', ['ngRoute'])

.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/', {
		templateUrl: '/app/home/home.html',
		controller: 'HomeCtrl'
	});
}])

.controller('HomeCtrl', ['$scope', '$timeout', 'blogService', 'photoService', function ($scope, $timeout, blogService, photoService) {
	$scope.navToggle = function () {
		$(".nav-toggle").toggleClass("active");
		$(".overlay-boxify").toggleClass("open");
	};

	$scope.blogEntries = blogService.query(function () {
		$timeout(function () {
			$scope.$broadcast('contentReady');
		});
	});

	var photoResponse = photoService.query(function () {
		$scope.photos = photoResponse;
	});
}]);