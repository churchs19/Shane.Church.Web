'use strict';

angular.module('shane.church', [
  'ngRoute',
  'smoothScroll',
  'shane.church.home',
  'shane.church.shared.waypoint',
  'shane.church.shared.flickity',
  'shane.church.shared.fancybox'
]).
config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
	$locationProvider.html5Mode(true);
	$routeProvider.otherwise({ redirectTo: '' });
}]);
