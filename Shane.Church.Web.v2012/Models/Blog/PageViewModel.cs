using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shane.Church.Web.v2012.Models.Blog
{
	public class PageViewModel
	{
		public PageViewModel()
		{

		}

		public IEnumerable<EntryViewModel> Entries { get; set; }
		public IEnumerable<ArchiveLinkModel> ArchiveLinks { get; set; }
		public IEnumerable<TagCloudItem> TagCloud { get; set; }
	}
}