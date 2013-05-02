using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Shane.Church.Web.v2012.Helpers
{
	public class RssResult : SyndicationResult
	{
		public RssResult(SyndicationFeed feed): base(new Rss20FeedFormatter(feed))
		{

		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "application/rss+xml";
				
			base.ExecuteResult(context);
		}
	}
}