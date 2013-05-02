using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using Shane.Church.Web.v2012.Models.Blog;
using Shane.Church.Web.v2012.Helpers;
using Shane.Church.Web.Data.Models;

namespace Shane.Church.Web.Controllers
{
	public class BlogController : Controller
	{
		public ActionResult Index(long id = -1)
		{
			if (id != -1)
			{
				return new PermanentRedirectResult("Entry", "Blog", new { id = id });
			}

			PageViewModel model = new PageViewModel();

			DataContext entityModel = new DataContext();
			var entries = (from e in entityModel.Journals
						   orderby e.EntryDate descending
						   select e.Id).Take(5);

			model.Entries = GetEntryModels(entries);
			model.ArchiveLinks = GetArchiveLinks();
			model.TagCloud = entityModel.GetBlogTagCloud(5);

			return View(model);
		}

		[OutputCache(Duration=0)]
		public ActionResult Entry(long id)
		{
			DataContext entityModel = new DataContext();
			var entry = (from e in entityModel.Journals
						   where e.Id == id
						   select e.Id);

			PageViewModel model = new PageViewModel();

			model.Entries = GetEntryModels(entry);
			model.ArchiveLinks = GetArchiveLinks();
			model.TagCloud = entityModel.GetBlogTagCloud(5);

			ViewBag.EntryTitle = model.Entries.FirstOrDefault().Entry.Title;

			return View(model);
		}

		public ActionResult Archive(int year, int month)
		{
			DataContext entityModel = new DataContext();
			var entries = (from e in entityModel.Journals
						   where e.EntryDate.Month == month && e.EntryDate.Year == year
						   orderby e.EntryDate descending
						   select e.Id);

			PageViewModel model = new PageViewModel();

			DateTime date = new DateTime(year, month, 1);

			ViewBag.Year = year.ToString();
			ViewBag.Month = date.ToString("MMMM");

			model.Entries = GetEntryModels(entries);
			model.ArchiveLinks = GetArchiveLinks();
			model.TagCloud = entityModel.GetBlogTagCloud(5);
	
			return View(model);
		}

		public ActionResult Tag(string id)
		{
			DataContext entityModel = new DataContext();
			var entries = (from t in entityModel.JournalTags
						   where t.Tag == id
						   orderby t.Journal.EntryDate descending
						   select t.Journal.Id);

			PageViewModel model = new PageViewModel();

			model.Entries = GetEntryModels(entries);
			model.ArchiveLinks = GetArchiveLinks();
			model.TagCloud = entityModel.GetBlogTagCloud(5);

			ViewBag.Tag = id;

			return View(model);
		}

		private IEnumerable<EntryViewModel> GetEntryModels(IEnumerable<long> ids)
		{
			List<EntryViewModel> entries = new List<EntryViewModel>();
			foreach (long id in ids)
			{
				EntryViewModel entry = new EntryViewModel(id);
				entries.Add(entry);
			}
			return entries;
		}

		private IEnumerable<ArchiveLinkModel> GetArchiveLinks()
		{
			List<ArchiveLinkModel> links = new List<ArchiveLinkModel>();
			DataContext model = new DataContext();
			var entryDates = (from j in model.Journals orderby j.EntryDate descending select j.EntryDate);
			List<DateTime> dateList = new List<DateTime>();
			foreach (DateTime d in entryDates)
			{
				dateList.Add(new DateTime(d.Year, d.Month, 1));
			}
			var dates = (from d in dateList
						 group d by d into g
						 orderby g.Key descending
						 select g.Key);

			foreach (DateTime d in dates)
			{
				links.Add(new ArchiveLinkModel() { Year = d.Year, Month = d.Month });
			}
			return links;
		}

		[CaptchaValidator]
		[HttpPost]
		public ActionResult PostComment(NewCommentModel comment)
		{
			if (ModelState.IsValid)
			{
				DataContext model = new DataContext();
				JournalComments newComment = new JournalComments();
				newComment.Comments = comment.Comments;
				newComment.Email = comment.Email;
				newComment.EntryDate = DateTime.Now;
				newComment.IPAddress = Request.UserHostAddress;
				newComment.JournalId = comment.ID;
				newComment.Link = comment.Link;
				newComment.Username = comment.UserName;

				model.JournalComments.Add(newComment);

				model.SaveChanges();

				//if (WebConfigurationManager.AppSettings["EmailComments"].Equals("true"))
				//{
				//	try
				//	{
				//		EntryViewModel postModel = new EntryViewModel(comment.ID);
				//		//Email notification
				//		string emailTo = postModel.Entry.User + "@s-church.net";
				//		string emailFrom = "support@s-church.net";
				//		string emailSubj = "New Comment on \"" + postModel.Entry.Title + "\"";
				//		string emailBody = "New comment on entry http://" + this.Request.Url.Authority + Url.Action("Entry", "Blog", new { id = comment.ID }) + "\n\n" +
				//			"User: " + comment.UserName + "\n" +
				//			"Email: " + comment.Email + "\n" +
				//			"Link: " + comment.Link + "\n" +
				//			"IP: " + Request.ServerVariables["REMOTE_ADDR"].ToString() + "\n\n" +
				//			"Comment:\n" + comment.Comments;

				//		string emailAccName = "shane@s-church.net";
				//		string emailAccPass = "Augu$t18o6!";

				//		MailMessage smtpMail = new MailMessage(emailFrom, emailTo, emailSubj, emailBody);

				//		// If you want to set the body to HTML
				//		smtpMail.IsBodyHtml = false;

				//		// Here configure an smtpclient object to connect to CT's smtp server
				//		SmtpClient mailClient = new SmtpClient("216.119.106.146");

				//		mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				//		// Set credentials to authenticate to CT's smtp server
				//		mailClient.Credentials = new NetworkCredential(emailAccName, emailAccPass);

				//		// Send email
				//		mailClient.Send(smtpMail);
				//	}
				//	catch { }
				//}

				return this.RedirectToAction("Entry", "Blog", new { id = comment.ID });				
			}
			return new HttpStatusCodeResult(500);
		}
	}
}
