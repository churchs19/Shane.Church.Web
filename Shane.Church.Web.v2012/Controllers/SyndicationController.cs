using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using Shane.Church.Web.v2012.Helpers;
using Shane.Church.Web.v2012.Models;
using Shane.Church.Web.Data.Models;

namespace Shane.Church.Web.Controllers
{
	public class SyndicationController : Controller
	{
		public SyndicationResult Blog(string id)
		{
			string title = "Shane Church - Blog";
			string description = "Shane's thoughts on software development and life in general";
			string link = "http://" + Request.Url.Authority + Url.Action("Index", "Blog");

			DataContext model = new DataContext();

			SyndicationFeed feed = new SyndicationFeed(title, description, new Uri(link));
			feed.LastUpdatedTime = (from j in model.Journals select j.EntryDate).Max();
			feed.ImageUrl = new Uri("http://" + Request.Url.Authority + Url.Content("~/Content/Images/side_nav_photo.jpg"));
			feed.Language = "en-us";
			feed.Copyright = new TextSyndicationContent("Copyright © Shane Church " + DateTime.Now.Year.ToString(), TextSyndicationContentKind.Plaintext);

			var authors = (from j in model.Journals
						   group j by j.User into g
						   select g.Key);
			foreach (string user in authors)
			{
				feed.Authors.Add(new SyndicationPerson(user.ToLower() + "@s-church.net", user, "http://" + Request.Url.Authority + Url.Content("~/")));
			}

			List<SyndicationItem> itemList = new List<SyndicationItem>();

			var posts = (from j in model.Journals
						 orderby j.EntryDate descending
						 select j).Take(5);
			foreach (Journal entry in posts)
			{
				SyndicationItem item = new SyndicationItem();
				item.Title = new TextSyndicationContent(entry.Title, TextSyndicationContentKind.Plaintext);
				item.Content = SyndicationContent.CreateHtmlContent(this.FixLocalLinks(this.GetEntry(entry.Image, entry.ImageText, entry.Entry)));
				item.Copyright = new TextSyndicationContent("Copyright © Shane Church " + entry.EntryDate.Year.ToString(), TextSyndicationContentKind.Plaintext);
				item.PublishDate = entry.EntryDate.ToUniversalTime();
				item.AddPermalink(new Uri("http://" + Request.Url.Authority + Url.Action("Entry", "Blog", new { id = entry.Id })));
				item.Authors.Add(new SyndicationPerson(entry.User.ToLower() + "@s-church.net", entry.User, "http://" + Request.Url.Authority + Url.Content("~/")));

				itemList.Add(item);
			}

			feed.Items = itemList;

			if (id == "Atom")
			{
				return new AtomResult(feed);
			}
			else
			{
				return new RssResult(feed);
			}
		}

		#region Blog Helpers
		private string StripHTMLTags(string strInput)
		{
			string strResult = Regex.Replace(strInput, @"<(.|\n)*?>", string.Empty);
			strResult = Regex.Replace(strResult, @"&nbsp;", " ");
			return strResult;
		}

		private string FixLocalLinks(string strInput)
		{
			string strResult = strInput;
			int i = 0;
			while (strResult.ToLower().IndexOf("<a href=\"", i) != -1)
			{
				i = strResult.ToLower().IndexOf("<a href=\"", i);
				i = i + 9;
				if (!strResult.Substring(i).StartsWith("http"))
				{
					strResult = strResult.Insert(i, "http://" + Request.Url.Authority + Url.Content("~/"));
				}
			}
			i = 0;
			while (strResult.ToLower().IndexOf("<img src=\"", i) != -1)
			{
				i = strResult.ToLower().IndexOf("<img src=\"", i);
				i = i + 10;
				if (!strResult.Substring(i).StartsWith("http"))
				{
					strResult = strResult.Insert(i, "http://" + Request.Url.Authority + Url.Content("~/"));
				}
			}
			return strResult;
		}

		private string GetEntry(string  image, string imageText, string entryText)
		{
			string sReturn = entryText.ToString();
			if (!string.IsNullOrEmpty(image))
			{
				sReturn = "<p align=\"center\"><img src=\"" + image.ToString() + "\" alt=\"" + imageText.ToString() + "\"></p>" + sReturn;
			}
			return sReturn;
		}
		#endregion

		public SyndicationResult Photos(string id, string categoryId = "")
		{
			DataContext model = new DataContext();

			string title = "Shane Church - Photos";
			string description = "Shane Church's most recent photos";
			string link = "http://" + Request.Url.Authority + Url.Action("Index", "PhotoAlbum");
			if (!String.IsNullOrEmpty(categoryId))
			{
				link = "http://" + Request.Url.Authority + Url.Action("Album", "PhotoAlbum", new { id = categoryId });
				string categoryTitle = (from p in model.Pages
										where p.PageId.ToLower() == categoryId.ToLower()
										select p.PageTitle).FirstOrDefault();
				title += " - " + categoryTitle;
			}
	
			SyndicationFeed feed = new SyndicationFeed(title, description, new Uri(link));
			feed.LastUpdatedTime = (from j in model.Photos select j.UpdatedDate).Max();
			feed.ImageUrl = new Uri("http://" + Request.Url.Authority + Url.Content("~/Content/Images/side_nav_photo.jpg"));
			feed.Language = "en-us";
			feed.Copyright = new TextSyndicationContent("Copyright © Shane Church " + DateTime.Now.Year.ToString(), TextSyndicationContentKind.Plaintext);
			feed.Authors.Add(new SyndicationPerson("shane@s-church.net", "Shane", "http://" + Request.Url.Authority + Url.Content("~/")));

			List<SyndicationItem> itemList = new List<SyndicationItem>();

			var photos = (from p in model.Photos
						  join pg in model.Pages on p.PageId equals pg.PageId
						  orderby p.UpdatedDate descending
						  select new PhotoViewModel() { ID = p.PhotoId, CategoryID = pg.PageId, CategoryName = pg.PageTitle, Caption = p.Caption, File = p.File, UpdatedDate = p.UpdatedDate }).Take(10);
			if (!String.IsNullOrEmpty(categoryId))
			{
				photos = (from p in model.Photos
						  join pg in model.Pages on p.PageId equals pg.PageId
						  where pg.PageId.ToLower() == categoryId.ToLower()
						  orderby p.UpdatedDate descending
						  select new PhotoViewModel() { ID = p.PhotoId, CategoryID = pg.PageId, CategoryName = pg.PageTitle, Caption = p.Caption, File = p.File, UpdatedDate = p.UpdatedDate }).Take(10);
			}
			foreach (PhotoViewModel entry in photos)
			{
				SyndicationItem item = new SyndicationItem();
				string permalink = "http://" + Request.Url.Authority + Url.Action("View", "PhotoAlbum", new { id = entry.ID });
				item.Title = new TextSyndicationContent(entry.CategoryName + " - " + entry.Caption, TextSyndicationContentKind.Plaintext);
				item.Content = SyndicationContent.CreateHtmlContent("<img src=\"" + "http://" + Request.Url.Authority + Url.Action("Image", "PhotoAlbum", new { id = entry.ID }) + "\" alt=\"" + entry.Caption + "\"/>");
				item.Copyright = new TextSyndicationContent("Copyright © Shane Church " + entry.UpdatedDate.Year.ToString(), TextSyndicationContentKind.Plaintext);
				item.PublishDate = entry.UpdatedDate.ToUniversalTime();
				item.AddPermalink(new Uri(permalink));
				item.Authors.Add(new SyndicationPerson("shane@s-church.net", "Shane", "http://" + Request.Url.Authority + Url.Content("~/")));

				itemList.Add(item);
			}

			feed.Items = itemList;

			if (id == "Atom")
			{
				return new AtomResult(feed);
			}
			else
			{
				return new RssResult(feed);
			}
		}
	}
}
