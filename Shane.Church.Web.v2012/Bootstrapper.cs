using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using Shane.Church.Web.Data;
using Shane.Church.Web.Cloud;
using Shane.Church.Web.Cloud.Azure;

namespace Shane.Church.Web.v2012
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>(); 
			container.RegisterType<IBlobStorage, AzureBlobStorage>();

            return container;
        }
    }
}