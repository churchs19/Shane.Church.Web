using System;
using System.Collections.Generic;

namespace Shane.Church.Web.Data.Models
{
	public class Journal
	{
		public Journal()
		{
			this.JournalComments = new List<JournalComments>();
			this.JournalTags = new List<JournalTags>();
		}

		public long Id { get; set; }
		public System.DateTime EntryDate { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
		public string ImageText { get; set; }
		public string Entry { get; set; }
		public string User { get; set; }
		public virtual ICollection<JournalComments> JournalComments { get; set; }
		public virtual ICollection<JournalTags> JournalTags { get; set; }
	}
}
