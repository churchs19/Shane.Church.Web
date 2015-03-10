'use strict';

angular.module('shane.church', [
  'ngRoute',
  'shane.church.home'
]).
config(['$routeProvider', function ($routeProvider) {
	$routeProvider.otherwise({ redirectTo: '/' });
}]);
