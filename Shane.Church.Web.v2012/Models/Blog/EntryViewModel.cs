using Shane.Church.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shane.Church.Web.v2012.Models.Blog
{
	public class EntryViewModel
	{
		public EntryViewModel(long id)
		{
			DataContext model = new DataContext();
			Entry = (from j in model.Journals
					 where j.Id == id
					 select j).FirstOrDefault();
			Comments = Entry.JournalComments.ToList();
			Tags = (from t in Entry.JournalTags
					select t.Tag);
		}

		public Journal Entry { get; set; }
		public IEnumerable<JournalComments> Comments { get; set; }
		public IEnumerable<string> Tags { get; set; }
	}
}