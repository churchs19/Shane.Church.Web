using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shane.Church.Web.v2012.Models;
using Shane.Church.Web.v2012.Models.Home;
using Shane.Church.Web.v2012.Helpers;
using Shane.Church.Web.Data.Models;

namespace Shane.Church.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			int maxJournalLength = 2500;

			IndexViewModel model = new IndexViewModel();

			DataContext entityModel = new DataContext();
			var blog = (from b in entityModel.Journals
						orderby b.EntryDate descending
						select b);
			if (blog.Count() > 0)
			{
				var latestBlog = blog.First();
				model.LatestBlogID = latestBlog.Id;
				model.LatestBlogDate = latestBlog.EntryDate.ToLongDateString();
				model.LatestBlogTime = latestBlog.EntryDate.ToShortTimeString();
				model.LatestBlogTitle = latestBlog.Title;
				model.LatestBlogImage = latestBlog.Image;
				model.LatestBlogImageText = latestBlog.ImageText;
				model.LatestBlogUser = latestBlog.User;
				model.LatestBlogTags = (from t in latestBlog.JournalTags
										select t.Tag);
				string entry = latestBlog.Entry;

				if (entry.Length > maxJournalLength)
				{
					int index = entry.ToLower().IndexOf("</p>", maxJournalLength);
					if (index != -1)
					{
						entry = entry.Substring(0, index);
						entry += "&nbsp;&nbsp;...</p>";
						model.LatestBlogTruncated = true;
					}
				}

				model.LatestBlogEntry = entry;
			}

			model.LatestPhotos = (from p in entityModel.Photos
								  join pg in entityModel.Pages on p.PageId equals pg.PageId
								  orderby p.UpdatedDate descending
								  select new PhotoViewModel() { ID = p.PhotoId, CategoryID = pg.PageId, CategoryName = pg.DisplayName, Caption = p.Caption, File = p.File, UpdatedDate = p.UpdatedDate }).Take(12);

			return View(model);
		}
	}
}
