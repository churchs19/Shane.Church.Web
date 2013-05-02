//jquery.Blog.js
//Copyright © Shane Church 2011

(function ($) {
	var methods = {
		init: function () {
			$("#main").addClass("blog_main");

			$(".blog_entry").Blog("evenRowHighlight");

			$("a.comments_disclosure").unbind("click").click(function () {
				$(this).parent().next().slideToggle();
				return false;
			});

			$("#modal-new-comment a.button-new-comment-submit").unbind("click").click(function () {
				$("#modal-new-comment form").removeData("validator");
				$("#modal-new-comment form").removeData("unobtrusiveValidation");
				$.validator.unobtrusive.parse("#modal-new-comment form");
				if ($("#modal-new-comment form").valid()) {

					var ajaxFormOptions = {
						cache: false,
						success: function (responseText, statusText, xhr, form) {
							var id = form[0].ID.value;

							$(".blog-id-" + id).find(".comments_panel ul").empty().append($(responseText).find(".comments_panel ul").children());

							$(".blog-id-" + id).find(".comments_disclosure").text($(responseText).find(".comments_disclosure").text());

							$(".blog-id-" + id).Blog("evenRowHighlight");

							$().HideModal({
								strId: "#modal-new-comment"
							});

							$("#modal-new-comment input,#modal-new-comment textarea").val("");
							$("#modal-new-comment .new-comment-message").text("");
							$("#modal-new-comment #recaptcha_area").removeClass("input-validation-error");
							Recaptcha.reload();
						},
						error: function (request, status, error) {
							$("#modal-new-comment .new-comment-message").text("Captcha response is invalid.");
							$("#modal-new-comment #recaptcha_area").addClass("input-validation-error");
						}
					};

					$("#modal-new-comment form").ajaxSubmit(ajaxFormOptions);
				}

				return false;
			});

			$("#modal-new-comment a.button-new-comment-cancel").click(function () {
				$().HideModal({
					strId: "#modal-new-comment"
				});

				$("#modal-new-comment input,#modal-new-comment textarea").val("");
				$("#modal-new-comment .new-comment-message").text("");
				$("#modal-new-comment #recaptcha_area").removeClass("input-validation-error");
				Recaptcha.reload();

				return false;
			});

			$("#modal-new-comment .button-close a").click(function () {
				$().HideModal({
					strId: "#modal-new-comment"
				});

				$("#modal-new-comment input,#modal-new-comment textarea").val("");
				$("#modal-new-comment .new-comment-message").text("");
				$("#modal-new-comment #recaptcha_area").removeClass("input-validation-error");
				Recaptcha.reload();

				return false;
			});

			$("a.new-comment-button").unbind("click").click(function () {
				$("#modal-new-comment input#ID").val($(this).attr("data-entry-id"));
				$().ShowModal({
					strId: "#modal-new-comment",
					callback: function () {
						var width = $("#modal-new-comment").width() - 20;
						$(".container-new-comment textarea, .container-new-comment input[type=text], .container-new-comment input[type=password]").width(width);
					}
				});

				return false;
			});

			SyntaxHighlighter.all();
		},
		evenRowHighlight: function () {
			return this.each(function () {
				var $this = $(this);
				$this.find(".comments_panel ul li").removeClass("even_comment");
				$this.find(".comments_panel ul li:even").addClass("even_comment");
			});
		}
	};

	$.fn.Blog = function (method) {
		if (methods[method]) {
			return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return methods.init.apply(this, arguments);
		} else {
			$.error('Method ' + method + ' does not exist on jQuery.Blog');
		}
	};

})(jQuery);
