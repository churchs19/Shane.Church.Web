//jquery.PhotoAlbum.js
//Copyright © Shane Church 2011

(function ($) {
	function resizeImageModal(image) {
		var $this = $(image);
		$this.css({
			height: "auto",
			width: "auto"
		});
		var maxWidth = $("#modal-view-photo .image").width() - 10;
		var maxHeight = $(window).height() - 40 - $("#modal-view-photo .caption").height();

		if ($(window).height() >= $(window).width()) {
			if ($this.outerWidth() > maxWidth) {
				$this.width(maxWidth);
			}
			if ($this.height() > maxHeight) {
				$this.height(maxHeight);
			}
		} else {
			if ($this.height() > maxHeight) {
				$this.height(maxHeight);
			}
			if ($this.outerWidth() > maxWidth) {
				$this.width(maxWidth);
			}
		}
		$("#modal-view-photo a.close").css("left", ($this.position().left + $this.width() - 2) + "px");
	}

	var methods = {
		init: function () { },
		loadPage: function (options) {
			var defaults = {
				href: "",
				callback: function () { }
			};

			var opts = $.extend(defaults, options);

			$.ajax({
				url: opts.href,
				type: "GET",
				dataType: 'html',
				cache: false,
				success: function (data) {
					var $data = $(data);

					$(".photos_container").empty();
					$(".photos_container").append($data.find(".photos_container").children());

					$(".pager").empty();
					$(".pager").append($data.find(".pager").children());

					opts.callback();
				},
				error: function (request, status, error) {
				}
			});
		},
		showPhoto: function (options) {
			var defaults = {
				href: "",
				title: "",
				category: "",
				file: "",
				callback: function () { }
			};

			var opts = $.extend(defaults, options);

			var $content = $("#modal-view-photo");
			$content.find(".image img").attr("src", opts.file).attr("alt", opts.category + " - " + opts.title);
			$content.find("h4").text(opts.title);
			$content.find("h5").text(opts.category);
			$content.find("a.close").unbind("click").click(function () {
				$().HideModal({ strId: "#modal-view-photo" });
				return false;
			});

			$("#modal-view-photo .image img").one("load", function () {
				resizeImageModal(this);
			});

			var modalOptions = {
				strId: "#modal-view-photo",
				callback: function () {
					resizeImageModal($("#modal-view-photo .image img")[0]);
				}
			};

			$().ShowModal(modalOptions);
		}
	};

	$.fn.PhotoAlbum = function (method) {
		if (methods[method]) {
			return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return methods.init.apply(this, arguments);
		} else {
			$.error('Method ' + method + ' does not exist on jQuery.PhotoAlbum');
		}
	};

})(jQuery);
