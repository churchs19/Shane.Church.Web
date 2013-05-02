using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shane.Church.Web.v2012.Helpers;

namespace Shane.Church.Web.Controllers
{
	public class LegacyController : Controller
	{
		//
		// GET: /Legacy/

		public ActionResult Index(string id)
		{

			switch(id.ToLower())
			{
				case "blog":
				case "journal":
					string isArchive = Request.Params["ARCHIVE"];
					string postId = Request.Params["ID"];
					string month = Request.Params["MONTH"];
					string year = Request.Params["YEAR"];
					if (postId == null)
						postId = Request.Params["JOURNAL_ID"];
					if (!String.IsNullOrEmpty(postId))
					{
						return new PermanentRedirectResult("Entry", "Blog", new { id = postId });
					}
					else
					{
						if (String.IsNullOrEmpty(isArchive) || isArchive.ToLower() == "n")
						{
							return new PermanentRedirectResult("Index", "Blog");
						}
						else
						{
							return new PermanentRedirectResult("Archive", "Blog", new { year = year, month = month });
						}
					}
				case "books":
					return new PermanentRedirectResult("Index", "ReadingList");
				case "booksrss":
					return new PermanentRedirectResult("Books", "Syndication");
				case "default":
					return new PermanentRedirectResult("Index", "Home");
				case "links":
					return new PermanentRedirectResult(); //Retired section
				case "login":
					return new PermanentRedirectResult("LogOn", "Account");
				case "palmos":
					return new PermanentRedirectResult("Palm", "Software");
				case "photo":
				case "photoalbum":
					string param = Request.Params["ID"];
					string page = Request.Params["PAGE"];
					if (page == null) { page = "0"; }
					if (param == null)
					{
						return new PermanentRedirectResult("Index", "PhotoAlbum");
					}
					else
					{
						return new PermanentRedirectResult("Album", "PhotoAlbum", new { id = param, pageNumber = page });
					}
				case "photorss":
					string categoryID = Request.QueryString["CATEGORY"];
					if (!String.IsNullOrEmpty(categoryID))
					{
						return new PermanentRedirectResult("Photos", "Syndication", new { categoryID = categoryID });
					}
					else
					{
						return new PermanentRedirectResult("Photos", "Syndication");
					}
				case "pocketpc":
					return new PermanentRedirectResult("MSPocketPC", "Software");
				case "pocketpcthemes":
					return new PermanentRedirectResult(); //Retired section
				case "rss":
					return new PermanentRedirectResult("Blog", "Syndication");
				case "rssfeeds":
					return new PermanentRedirectResult(); //Retired section
				case "smartphonesoftware":
					return new PermanentRedirectResult("WMSmartphone", "Software");
				case "viewphoto":
					string photoId = Request.QueryString["ID"];
					if (!String.IsNullOrEmpty(photoId))
					{
						return new PermanentRedirectResult("View", "PhotoAlbum", new { id = photoId });
					}
					else
					{
						return new PermanentRedirectResult();
					}
				case "windows":
					return new PermanentRedirectResult("MSWindows", "Software");
				case "windowsmobilesoftware":
					return new PermanentRedirectResult("WMPocketPC", "Software");
			}
			return new PermanentRedirectResult();
		}

	}
}
