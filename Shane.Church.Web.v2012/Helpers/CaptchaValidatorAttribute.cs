﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Shane.Church.Web.v2012.Helpers
{
	public class CaptchaValidatorAttribute : ActionFilterAttribute
	{
		private const string CHALLENGE_FIELD_KEY = "recaptcha_challenge_field";
		private const string RESPONSE_FIELD_KEY = "recaptcha_response_field";

		public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var captchaChallengeValue = filterContext.HttpContext.Request.Form[CHALLENGE_FIELD_KEY];
            var captchaResponseValue = filterContext.HttpContext.Request.Form[RESPONSE_FIELD_KEY];
            var captchaValidtor = new Recaptcha.RecaptchaValidator
                                      {
                                          PrivateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"],
                                          RemoteIP = filterContext.HttpContext.Request.UserHostAddress,
                                          Challenge = captchaChallengeValue,
                                          Response = captchaResponseValue
                                      };

            var recaptchaResponse = captchaValidtor.Validate();

			// this will push the result value into a parameter in our Action
            //filterContext.ActionParameters["captchaValid"] = recaptchaResponse.IsValid;
			if (!recaptchaResponse.IsValid)
			{
				filterContext.Controller.ViewData.ModelState.AddModelError("ReCaptcha", "Captcha response is invalid");
			}

            base.OnActionExecuting(filterContext);
        }
	}
}