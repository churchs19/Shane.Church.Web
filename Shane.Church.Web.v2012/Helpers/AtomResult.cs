using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;
using System.Web.Mvc;

namespace Shane.Church.Web.v2012.Helpers
{
	public class AtomResult : SyndicationResult
	{
		public AtomResult(SyndicationFeed feed): base(new Atom10FeedFormatter(feed))
		{

		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "application/rss+xml";
				
			base.ExecuteResult(context);
		}
	}
}