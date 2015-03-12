'use strict';

angular.module('shane.church', [
  'ngRoute',
  'shane.church.home',
  'shane.church.shared.waypoint'
]).
config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
	$locationProvider.html5Mode(true);
	$routeProvider.otherwise({ redirectTo: '' });
}]);
