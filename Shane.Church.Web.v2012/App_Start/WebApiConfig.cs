using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Shane.Church.Web.v2012
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Filters.Add(new Shane.Church.Web.v2012.Utility.ExceptionHandlerFilter());

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
