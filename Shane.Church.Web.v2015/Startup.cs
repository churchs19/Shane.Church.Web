using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Logging;

namespace Shane.Church.Web.v2015
{
    public class Startup
    {
		public IConfiguration Configuration { get; private set; }

		public Startup()
		{
		}

		public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
			app.UseStaticFiles();
			DefaultFilesOptions dfOpts = new DefaultFilesOptions();
			dfOpts.DefaultFileNames.Add("index.html");
			app.UseDefaultFiles(dfOpts);
			app.UseMvc();
        }
    }
}
