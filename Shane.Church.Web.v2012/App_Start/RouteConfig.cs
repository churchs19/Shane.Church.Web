using System.Web.Mvc;
using System.Web.Routing;

namespace Shane.Church.Web.v2012
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
			routes.IgnoreRoute("{*allasmx}", new { allasmx = @".*\.asmx(/.*)?" });
			routes.IgnoreRoute("{*allsvc}", new { allsvc = @".*\.svc(/.*)?" });
			routes.IgnoreRoute("*wedding*");

			routes.MapRoute(
				"{*allaspx}",
				@"{id}.aspx",
				new { controller = "Legacy", action = "Index" });

			RegisterSoftwareRoutes(routes);

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		private static void RegisterSoftwareRoutes(RouteCollection routes)
		{
			//Begin Software Routes
			#region Windows
			routes.MapRoute(
				"Windows",
				"Windows",
				new { controller = "Software", action = "MSWindows", id = 0 });

			routes.MapRoute(
				"BasecampPeopleSync",
				"BasecampPeopleSync",
				new { controller = "Software", action = "MSWindows", id = 1 });

			routes.MapRoute(
				"StirlingWeatherTray",
				"StirlingWeather/Tray",
				new { controller = "Software", action = "MSWindows", id = 2 });

			routes.MapRoute(
				"iTunesPlaylist",
				"iTunes",
				new { controller = "Software", action = "MSWindows", id = 3 });
			#endregion

			#region Gadgets
			routes.MapRoute(
				"Gadgets",
				"Gadgets",
				new { controller = "Software", action = "Win7Gadgets", id = 0 });

			routes.MapRoute(
				"StirlingWeatherGadget",
				"StirlingWeather/Gadget",
				new { controller = "Software", action = "Win7Gadgets", id = 1 });
			#endregion

			#region Smartphone
			routes.MapRoute(
				"Smartphone",
				"WindowsMobile/Smartphone",
				new { controller = "Software", action = "WMSmartphone", id = 0 });

			routes.MapRoute(
				"StirlingWeatherSmartphone",
				"StirlingWeather/Smartphone",
				new { controller = "Software", action = "WMSmartphone", id = 1 });

			routes.MapRoute(
				"PyramidSmartphone",
				"Pyramid/Smartphone",
				new { controller = "Software", action = "WMSmartphone", id = 2 });

			routes.MapRoute(
				"ClockSolitaireSmartphone",
				"ClockSolitaire/Smartphone",
				new { controller = "Software", action = "WMSmartphone", id = 3 });

			routes.MapRoute(
				"FourCornersSmartphone",
				"FourCorners/Smartphone",
				new { controller = "Software", action = "WMSmartphone", id = 4 });

			routes.MapRoute(
				"StirlingBlogSmartphone",
				"StirlingBlog/Smartphone",
				new { controller = "Software", action = "WMSmartphone", id = 5 });

			routes.MapRoute(
				"StirlingBirthdaySmartphone",
				"StirlingBirthday/Smartphone",
				new { controller = "Software", action = "WMSmartphone", id = 6 });
			#endregion

			#region WM5PocketPC
			routes.MapRoute(
				"WM5PocketPC",
				"WindowsMobile/PocketPC",
				new { controller = "Software", action = "WMPocketPC", id = 0 });

			routes.MapRoute(
				"StirlingWeatherPocketPC",
				"StirlingWeather/PocketPC",
				new { controller = "Software", action = "WMPocketPC", id = 1 });

			routes.MapRoute(
				"PyramidPocketPC",
				"Pyramid/PocketPC",
				new { controller = "Software", action = "WMPocketPC", id = 2 });

			routes.MapRoute(
				"ClockSolitairePocketPC",
				"ClockSolitaire/PocketPC",
				new { controller = "Software", action = "WMPocketPC", id = 3 });

			routes.MapRoute(
				"FourCornersPocketPC",
				"FourCorners/PocketPC",
				new { controller = "Software", action = "WMPocketPC", id = 4 });

			routes.MapRoute(
				"CurrencyConverter2005PocketPC",
				"CurrencyConverter2005/PocketPC",
				new { controller = "Software", action = "WMPocketPC", id = 5 });

			routes.MapRoute(
				"StrikeOutPocketPC",
				"StrikeOut/PocketPC",
				new { controller = "Software", action = "WMPocketPC", id = 6 });
			#endregion

			#region MSPocketPC
			routes.MapRoute(
				"PocketPC",
				"PocketPC",
				new { controller = "Software", action = "MSPocketPC", id = 0 });

			routes.MapRoute(
				"PPCCurrencyConverter",
				"CurrencyConverter/PocketPC",
				new { controller = "Software", action = "MSPocketPC", id = 1 });

			routes.MapRoute(
				"PPCChess",
				"Chess/PocketPC",
				new { controller = "Software", action = "MSPocketPC", id = 2 });

			routes.MapRoute(
				"LangtonsAnt",
				"LangtonsAnt/PocketPC",
				new { controller = "Software", action = "MSPocketPC", id = 3 });
			#endregion

			#region PalmOS
			routes.MapRoute(
				"PalmOS",
				"PalmOS",
				new { controller = "Software", action = "Palm" });
			#endregion

			#region Windows Phone
			routes.MapRoute(
			"WindowsPhone",
			"WindowsPhone",
			new { controller = "Software", action = "WindowsPhone", id = 0 });

			routes.MapRoute(
				"StirlingMoney",
				"StirlingMoney/WindowsPhone",
				new { controller = "Software", action = "WindowsPhone", id = 1 });

			routes.MapRoute(
				"WhatIEatWP",
				"WhatIEat/WindowsPhone",
				new { controller = "Software", action = "WindowsPhone", id = 2 });

			routes.MapRoute(
				"StirlingBirthday",
				"StirlingBirthday/WindowsPhone",
				new { controller = "Software", action = "WindowsPhone", id = 3 });
			#endregion

			//End Software Routes
		}
	}
}