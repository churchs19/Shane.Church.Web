using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shane.Church.Web.v2012.Models.Blog;

namespace Shane.Church.Web.v2012.Models.Home
{
	public class IndexViewModel
	{
		public IndexViewModel()
		{
			LatestBlogTruncated = false;
			LatestPhotos = new List<PhotoViewModel>();
		}

		public long LatestBlogID { get; set; }
		public string LatestBlogTitle { get; set; }
		public string LatestBlogDate { get; set; }
		public string LatestBlogTime { get; set; }
		public string LatestBlogEntry { get; set; }
		public bool LatestBlogTruncated { get; set; }
		public bool LatestBlogHasImage
		{
			get
			{
				return !String.IsNullOrEmpty(LatestBlogImageText);
			}
		}
		public string LatestBlogImage { get; set; }
		public string LatestBlogImageText { get; set; }
		public string LatestBlogUser { get; set; }
		public IEnumerable<string> LatestBlogTags { get; set; }

		public IEnumerable<PhotoViewModel> LatestPhotos { get; set; }
	}
}