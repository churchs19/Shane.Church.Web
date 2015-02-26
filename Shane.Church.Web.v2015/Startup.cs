using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.ConfigurationModel;

namespace Shane.Church.Web.v2015
{
    public class Startup
    {
		public IConfiguration Configuration { get; private set; }

		public Startup()
		{
			Configuration = new Configuration().AddJsonFile("config.json");
		}

		public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
			app.UseMvc();
            app.UseWelcomePage();
        }
    }
}
