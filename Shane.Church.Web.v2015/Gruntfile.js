/// <binding BeforeBuild='default' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
	grunt.initConfig({
		bower: {
			install: {
				options: {
					targetDir: "wwwroot/lib",
					layout: "byComponent",
					cleanTargetDir: true
				}
			}
		},
		modernizr: {
			dist: {
				"devFile": "wwwroot/js/modernizr-2.8.3.js",
				"outputFile": "wwwroot/lib/modernizr/modernizr-2.8.3.js",
				"files": {
					"src": ["wwwroot/js"]
				}
			}
		}
	});

	grunt.registerTask("default", ["bower:install", "modernizr:dist"]);

	grunt.loadNpmTasks("grunt-bower-task");

	grunt.loadNpmTasks("grunt-modernizr");
};