using System.Web.Optimization;

namespace Shane.Church.Web.v2012
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/jquery.textshadow.js",
						"~/Scripts/Shane.Church/jquery.Navigation.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.unobtrusive*",
						"~/Scripts/jquery.validate*",
						"~/Scripts/jquery.form.js"));

			bundles.Add(new ScriptBundle("~/bundles/pinify").Include(
						"~/Scripts/jquery.pinify.js",
						"~/Scripts/jquery.pinable.js",
						"~/Scripts/Shane.Church/jquery.DynamicJumpList.js"));

			bundles.Add(new ScriptBundle("~/bundles/jquerytools").Include(
						"~/Scripts/jquery.tools.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/shanechurch/modal").Include(
						"~/Scripts/Shane.Church/jquery.Modal.js"));

			bundles.Add(new ScriptBundle("~/bundles/shanechurch/blog").Include(
						"~/Scripts/Shane.Church/jquery.Blog.js"));

			bundles.Add(new ScriptBundle("~/bundles/shanechurch/googlemap").Include(
						"~/Scripts/Shane.Church/jquery.GoogleMap.js",
						"~/Scripts/infobox.js"));

			bundles.Add(new ScriptBundle("~/bundles/shanechurch/photoalbum").Include(
						"~/Scripts/Shane.Church/jquery.PhotoAlbum.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
						"~/Content/Normalize.css",
						"~/Content/jquery.text-shadow.css",
						"~/Content/discoverability.css",
						"~/Content/ResponsiveSite.css"));

			bundles.Add(new StyleBundle("~/Content/syntaxhighlighter").Include(
							"~/Content/SyntaxHighlighter/shCore.css",
							"~/Content/SyntaxHighlighter/shCoreDefault.css",
							"~/Content/SyntaxHighlighter/shThemeDefault.css"));
		}
	}
}