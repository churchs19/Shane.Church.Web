using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shane.Church.Web.v2012.Helpers
{
	public class PermanentRedirectResult: ActionResult
	{
		private string _action = null;
		private string _controller = null;
		private object _routeValues = new { };

		public PermanentRedirectResult()
		{

		}

		public PermanentRedirectResult(string Action, string Controller)
		{
			_action = Action;
			_controller = Controller;
		}

		public PermanentRedirectResult(string Action, string Controller, object RouteValues)
		{
			_action = Action;
			_controller = Controller;
			_routeValues = RouteValues;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (_action == null || _controller == null)
			{
				context.HttpContext.Response.Status = "404 Not Found";
				context.HttpContext.Response.StatusCode = 404;
			}
			else
			{
				context.HttpContext.Response.Status = "301 Moved Permanently";
				context.HttpContext.Response.StatusCode = 301;
				context.HttpContext.Response.AppendHeader("Location", ((Controller)(context.Controller)).Url.Action(_action, _controller, _routeValues));
			}
		}
	}
}