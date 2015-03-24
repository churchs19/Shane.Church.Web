'use strict';

angular.module('shane.church', [
  'ngResource',
  'ngRoute',
  'smoothScroll',
  'shane.church.home',
  'shane.church.shared.waypoint',
  'shane.church.shared.fancybox',
  'shane.church.shared.flickity',
  'shane.church.shared.photoItem',
  'shane.church.services'
]).
config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
	$locationProvider.hashPrefix('!');
	$routeProvider.otherwise({ redirectTo: '' });
}]);
