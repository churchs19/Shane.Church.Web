using Shane.Church.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using AutoMapper;

namespace Shane.Church.Web.v2012
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			Bootstrapper.Initialize();

			Mapper.CreateMap<Shane.Church.Web.Data.Models.Page, Shane.Church.Web.Common.DataTransfer.PhotoCategoryDTO>();
			Mapper.CreateMap<Shane.Church.Web.Data.Models.Photo, Shane.Church.Web.Common.DataTransfer.PhotoDTO>();
			Mapper.CreateMap<Shane.Church.Web.Common.DataTransfer.PhotoCategoryDTO, Shane.Church.Web.Data.Models.Page>();
			Mapper.CreateMap<Shane.Church.Web.Common.DataTransfer.PhotoDTO, Shane.Church.Web.Data.Models.Photo>();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();
		}
	}
}