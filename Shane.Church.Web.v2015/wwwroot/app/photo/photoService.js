var serviceModule = serviceModule || angular.module('shane.church.services', ['ngResource']);

serviceModule.factory('photoService', ['$resource', function ($resource) {
	return $resource('/api/photo/:id');
}]);