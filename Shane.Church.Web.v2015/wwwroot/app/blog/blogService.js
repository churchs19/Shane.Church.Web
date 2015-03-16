var serviceModule = serviceModule || angular.module('shane.church.services', ['ngResource']);

serviceModule.factory('blogService', ['$resource', function ($resource) {
	return $resource('/api/blogentry/:id');
}]);