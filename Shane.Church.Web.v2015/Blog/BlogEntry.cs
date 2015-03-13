using Shane.Church.Web.Data.Models;
using Shane.Church.Web.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Shane.Church.Web.Blog
{
    public class BlogEntry
    {
		public BlogEntry()
		{
			Tags = new List<string>();
			Comments = new List<BlogEntryComment>();
		}

		public long Id { get; set; }
		public System.DateTime EntryDate { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
		public string ImageText { get; set; }
		public string Entry { get; set; }
		public string User { get; set; }
		public List<BlogEntryComment> Comments { get; set; }
		public List<string> Tags { get; set; }

		public static BlogEntry fromJournal(Journal item, bool summary = false)
		{
			BlogEntry entry = new BlogEntry();
			entry.Id = item.Id;
			entry.EntryDate = item.EntryDate;
			entry.Image = item.Image;
			entry.ImageText = item.ImageText;
			entry.Entry = summary ? item.Entry.StripTags().TruncateHtml() : item.Entry;
			entry.User = item.User;

			entry.Comments = new List<BlogEntryComment>();

			if (!summary)
			{
				entry.Tags.AddRange(item.JournalTags.Select(it => it.Tag));

				foreach (var itemComment in item.JournalComments)
				{
					entry.Comments.Add(BlogEntryComment.fromJournalComment(itemComment));
				}
			}

			return entry;
		}
	}
}