using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Configuration;
using System.Web.Mvc;

namespace Shane.Church.Web.v2012.Helpers
{
	public static class RecaptchaHtmlHelper
	{
		public static string GenerateCaptcha(this HtmlHelper helper, string theme = "white")
		{
			var captchaControl = new Recaptcha.RecaptchaControl
					{
						ID = "recaptcha",
						Theme = theme,
						PublicKey = ConfigurationManager.AppSettings["RecaptchaPublicKey"],
						PrivateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"]
					};

			var htmlWriter = new HtmlTextWriter(new StringWriter());

			captchaControl.RenderControl(htmlWriter);

			return htmlWriter.InnerWriter.ToString();
		}
	}
}