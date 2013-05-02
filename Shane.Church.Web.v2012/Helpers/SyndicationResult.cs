using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Shane.Church.Web.v2012.Helpers
{
	public class SyndicationResult : ActionResult
	{
		public SyndicationFeedFormatter Formatter { get; set; }

		public SyndicationResult(SyndicationFeedFormatter formatter)
		{
			Formatter = formatter;
		}
	
		public override void  ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddHours(1));
			context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.HttpContext.Response.Cache.SetValidUntilExpires(true);
	
			using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
			{
				Formatter.WriteTo(writer);
			}
		}
	}
}