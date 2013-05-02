using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Shane.Church.Web.v2012.Utility
{
	/// <summary>
	/// Filter to process all exceptions in the API Controllers.
	/// This is initialized in the <see cref="ElPaso.Assessor.OilAndGas.BusinessServices.FilterConfig">FilterConfig</see>
	/// </summary>
	public class ExceptionHandlerFilter : ExceptionFilterAttribute
	{
		public ExceptionHandlerFilter()
		{
			this.Mappings = new Dictionary<Type, HttpStatusCode>();
			this.Mappings.Add(typeof(ArgumentNullException), HttpStatusCode.BadRequest);
			this.Mappings.Add(typeof(ArgumentException), HttpStatusCode.BadRequest);
			this.Mappings.Add(typeof(NotImplementedException), HttpStatusCode.NotImplemented);
		}

		public IDictionary<Type, HttpStatusCode> Mappings { get; private set; }

		/// <summary>
		/// Processes any unhandled exception thrown by any controller message.
		/// Logs the exception and then builds the appropriate HTTP Response
		/// </summary>
		/// <param name="actionExecutedContext">The context for the request that threw the exception</param>
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			if (actionExecutedContext.Exception != null)
			{
				var exception = actionExecutedContext.Exception;

				if (actionExecutedContext.Exception is HttpException)
				{
					var httpException = (HttpException)exception;
					actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse((HttpStatusCode)httpException.GetHttpCode(), httpException.Message);
				}
				else if (this.Mappings.ContainsKey(exception.GetType()))
				{
					var httpStatusCode = this.Mappings[exception.GetType()];
					actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(httpStatusCode, exception.Message);
				}
				else
				{
					actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
				}
			}
		}
	}
}