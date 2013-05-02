using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shane.Church.Web.v2012.Models.PhotoAlbum
{
	public class PageItem
	{
		public string ID { get; set; }
		public string DisplayName { get; set; }
		public string Title { get; set; }
		public long SortOrder { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public DateTime UpdatedDate { get; set; }
		public int Count { get; set; }
	}

	public class IndexViewModel
	{
		public IndexViewModel()
		{
			Pages = new List<PageItem>();
		}

		public IEnumerable<PageItem> Pages { get; set; }
	}
}