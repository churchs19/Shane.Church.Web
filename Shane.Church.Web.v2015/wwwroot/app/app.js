'use strict';

angular.module('shane.church', [
  'ngResource',
  'ngRoute',
  'smoothScroll',
  'shane.church.home',
  'shane.church.shared',
  'shane.church.services'
]).
config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
	$locationProvider.hashPrefix('!');
	$routeProvider.otherwise({ redirectTo: '' });
}]);
