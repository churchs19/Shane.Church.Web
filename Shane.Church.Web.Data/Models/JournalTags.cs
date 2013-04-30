using System;
using System.Collections.Generic;

namespace Shane.Church.Web.Data.Models
{
	public class JournalTags
	{
		public long JournalTagId { get; set; }
		public long JournalId { get; set; }
		public string Tag { get; set; }
		public virtual Journal Journal { get; set; }
	}
}
