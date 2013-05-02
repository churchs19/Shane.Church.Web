//jquery.DynamicJumpList.js
//Copyright © Shane Church 2011

(function ($) {
	var methods = {
		init: function (options) {
			var defaults = {
				feeds: [],
				debug: false,
				callback: function () { }
			};

			var opts = $.extend(defaults, options);

			if (opts.debug || $.pinify.isPinned()) {
				for (var i = 0; i < opts.feeds.length; i++) {
					var feed = {
						jumpListTitle: opts.feeds[i].jumpListTitle,
						feedUrl: opts.feeds[i].feedUrl,
						iconUrl: opts.feeds[i].iconUrl,
						overlayIconUrl: opts.feeds[i].overlayIconUrl,
						overlayIconTooltip: opts.feeds[i].overlayIconTooltip
					};
					$().DynamicJumpList("updateJumpList", feed);
					setTimeout(function () {
						$().DynamicJumpList("updateJumpList", feed);
					}, 600000);
				}
			}

			opts.callback();
		},
		updateJumpList: function (options) {
			var defaults = {
				jumpListTitle: "",
				feedUrl: "",
				iconUrl: "",
				overlayIconUrl: "",
				overlayIconTooltip: "",
				callback: function () { }
			};

			var opts = $.extend(defaults, options);

			if (opts.feedUrl !== "") {
				$.ajax({
					url: opts.feedUrl,
					type: "GET",
					dataType: 'xml',
					cache: false,
					success: function (data) {
						var $data = $(data);

						var itemsArray = [];

						var maxPubDate = new Date(0);

						$data.find("item").each(function () {
							var $this = $(this);

							var item = {
								'name': $this.find("title").text(),
								'url': $this.find("link").text(),
								'icon': opts.iconUrl
							};

							var pubDate = Date.parse($this.find("pubDate").text());
							if (pubDate > maxPubDate) {
								maxPubDate = pubDate;
							}

							itemsArray.push(item);
						});

						itemsArray.reverse();

						$.pinify.addJumpList({
							title: opts.jumpListTitle,
							items: itemsArray
						});

						var lastReadDate = $.cookie('last_read_' + document.domain);
						if (lastReadDate === null || new Date(parseInt(lastReadDate, 10)) < new Date(maxPubDate)) {
							$.pinify.addOverlay({
								icon: opts.overlayIconUrl,
								title: opts.overlayIconTooltip
							});
						}

						opts.callback();
					},
					error: function (request, status, error) {
					}
				});
			}
		}
	};


	$.fn.DynamicJumpList = function (method) {
		if (methods[method]) {
			return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return methods.init.apply(this, arguments);
		} else {
			$.error('Method ' + method + ' does not exist on jQuery.DynamicJumpList');
		}
	};

})(jQuery);
