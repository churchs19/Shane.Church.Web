using System.Web;
using System.Web.Mvc;

namespace Shane.Church.Web.v2012
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}