/*
Modal window plugins
*/
(function ($) {

	// Show a modal dialog.
	$.fn.ShowModal = function (options) {
		var defaults = {
			strId: "",
			intFadeSpeed: 300,
			callback: function () { }
		};
		var opts = $.extend(defaults, options);
		if (!jQuery.support.opacity) {
			// Yeah, no, IE doesn't do fading so well with alpha transparent PNGs.
			opts.intFadeSpeed = 0;
		}
		var dims = {};
		var offset = {};

		// "grey out" the main window.
		// Set the new modal window with a known z-index that we know will surpass everything.
		var intHeight = $(document).height() - 2;
		$("#modal-background").show().height(intHeight);

		if($(window).width() <= 577) {
			$(opts.strId).css({
				"z-index": "11000",
				"top": ($(window).scrollTop()) + "px",
//				"left": "0px",
				"left": (($(window).width() - $(opts.strId).outerWidth()) / 2) + "px",
				"height": ($(window).height() - 10) + "px",
				"max-width": ($(window).width() - 10) + "px"
			});
		} else {
			$(opts.strId).css({
				"z-index": "11000",
				"top": ($(window).scrollTop() + ($(window).height() / 10)) + "px",
				"left": (($(window).width() - $(opts.strId).outerWidth()) / 2) + "px",
				"height": "auto",
				"max-width": ($(window).width() - 10) + "px"
			});
		}

		// Show the new modal
		$(opts.strId).fadeIn(opts.intFadeSpeed, function () {
			// Execute callback, if any
			opts.callback();
		});
	};

	// Hide a modal window.
	$.fn.HideModal = function (options) {
		// Because modal windows can stack, this means hiding both the top modal and the "grey out" layer just underneath.
		// First, hide the topmost modal, then on callback...
		var defaults = {
			strId: "",
			intFadeSpeed: 300,
			callback: function () { }
		};
		var opts = $.extend(defaults, options);
		if (!jQuery.support.opacity) {
			opts.intFadeSpeed = 0;
		}
		$(opts.strId).fadeOut(opts.intFadeSpeed, function () {
			// This was the only modal, so hide the "grey out" layer that covers everything.
			$("#modal-background").hide();

			// special catch for generic modal: we need to clear its content so that future appends start with a clean slate.
			if (opts.strId == "#modal-generic") {
				$(opts.strId + " div.content div.content-padding").empty();
			}

			$(window).off("scroll");

			// Execute callback, if any
			opts.callback();
			return this;
		});
	};

	// Show a "generic" modal (basically a blank modal with more default items)
	$.fn.ShowGenericModal = function (options) {
		var defaults = {
			strTitle: "",
			strDialogContent: "",
			callback: function () { }
		};
		var opts = $.extend(defaults, options);
		opts.strId = "#modal-generic";
		$().ShowModal({ strId: opts.strId, callback: opts.callback });
		$(opts.strId + " .top .button-close a").unbind().click(function () {
			$().HideGenericModal({ strId: opts.strId, callback: opts.callback });
		});
		$(opts.strId + " div.top h3").text(opts.strTitle);
		$(opts.strId + " div.modal-content").append(opts.strDialogContent);
	};

	// Hide a "generic" modal
	$.fn.HideGenericModal = function (options) {
		var defaults = {
			callback: function () { }
		};
		var opts = $.extend(defaults, options);
		var thisId = "#modal-generic";
		$().HideModal({
			strId: thisId,
			callback: function () {
				$("#modal-generic div.modal-content").empty();
				opts.callback();
			}
		});
	};
})(jQuery);