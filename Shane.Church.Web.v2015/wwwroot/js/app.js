'use strict';

angular.module('shane.church', [
  'ngResource',
  'ngRoute',
  'smoothScroll',
  'shane.church.home',
  'shane.church.shared.waypoint',
  'shane.church.shared.flickity',
  'shane.church.shared.fancybox',
  'shane.church.services'
]).
config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
	$locationProvider.html5Mode({ requireBase: false });
	$routeProvider.otherwise({ redirectTo: '' });
}]);
