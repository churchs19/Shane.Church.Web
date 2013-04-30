using System;
using System.Collections.Generic;
using System.Text;

namespace Shane.Church.Web.Common.DataTransfer
{
	public class PhotoCategoryDTO
	{
		public string PageId { get; set; }
		public string DisplayName { get; set; }
		public string PageTitle { get; set; }
		public long SortOrder { get; set; }
		public Nullable<short> Active { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
	}
}
