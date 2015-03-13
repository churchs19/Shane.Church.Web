using Shane.Church.Web.Data.Models;
using System;

namespace Shane.Church.Web.Blog
{
    public class BlogEntryComment
    {
		public long Id { get; set; }
		public DateTime EntryDate { get; set; }
		public string Username { get; set; }
		public string Link { get; set; }
		public string Comments { get; set; }

		public static BlogEntryComment fromJournalComment(JournalComments item)
		{
			BlogEntryComment comment = new BlogEntryComment();
			comment.Id = item.CommentId;
			comment.EntryDate = item.EntryDate != null ? item.EntryDate.Value : DateTime.MinValue;
			comment.Username = item.Username;
			comment.Link = item.Link;
			comment.Comments = item.Comments;

			return comment;
		}
	}
}