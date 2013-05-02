using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shane.Church.Web.Data.Models;

namespace Shane.Church.Web.v2012.Models.Blog
{
	public class CommentsViewModel
	{
		public CommentsViewModel()
		{

		}

		public long JournalID { get; set; }
		public IEnumerable<JournalComments> Comments { get; set; }
	}
}