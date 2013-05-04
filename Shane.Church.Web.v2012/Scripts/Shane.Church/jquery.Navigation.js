//jquery.Navigation.js
//Copyright © Shane Church 2012

(function ($) {
	var menuTimeout = null;

	var methods = {
		init: function () {
			$("ul.ssc_nav li").mouseenter(function () {
				var $this = $(this);
				$this.find(".ssc_sub_nav").slideDown(200, function () {
					menuTimeout = window.setTimeout(function () {
						$this.find(".ssc_sub_nav").slideUp(200);
					}, 1500);
				});
			});

			$("ul.ssc_nav li > a").click(function (event) {
				var $subNav = $(this).parent().find(".ssc_sub_nav");
				if ($subNav.length > 0) {
					event.preventDefault();
					$subNav.slideDown(200, function () {
						menuTimeout = window.setTimeout(function () {
							$subNav.slideUp(200);
						}, 2500);
					});
				}
			});

			$(".ssc_sub_nav a").mouseenter(function () {
				window.clearTimeout(menuTimeout);
				menuTimeout = null;
			});

			$(".ssc_sub_nav a").mouseleave(function () {
				window.clearTimeout(menuTimeout);
				menuTimeout = null;
				var $this = $(this);
				menuTimeout = window.setTimeout(function () {
					$this.parent().slideUp(200);
				}, 500);
			});

			$("select.ssc_nav, select.archive_links, select.photo_links, select.tagCloud").change(function () {
				if ($(this).val().length > 0) {
					window.location = $(this).val();
				}
			});
		},
		initTabs: function (options) {
			var defaults = {
				selectedIndex: 0
			};

			var opts = $.extend(defaults, options);

			$("ul.tabs").tabs("div.panes > div");

			if (opts.selectedIndex >= 1) {
				$("ul.tabs").data("tabs").click(opts.selectedIndex - 1);
			}

			$(".mini-tabs select").change(function () {
				if ($(this).val().length > 0) {
					$("ul.tabs").data("tabs").click(parseInt($(this).val()) - 1);
				}
			});
		}
	};

	$.fn.Navigation = function (method) {
		if (methods[method]) {
			return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return methods.init.apply(this, arguments);
		} else {
			$.error('Method ' + method + ' does not exist on jQuery.Blog');
		}
	};
})(jQuery);
