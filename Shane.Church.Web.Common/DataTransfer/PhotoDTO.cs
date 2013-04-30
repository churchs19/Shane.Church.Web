using System;
using System.Collections.Generic;
using System.Text;

namespace Shane.Church.Web.Common.DataTransfer
{
	public class PhotoDTO
	{
		public long PhotoId { get; set; }
		public string PageId { get; set; }
		public string Caption { get; set; }
		public string File { get; set; }
		public Nullable<long> DisplayWidth { get; set; }
		public Nullable<long> DisplayHeight { get; set; }
		public Nullable<long> SortOrder { get; set; }
		public System.DateTime UpdatedDate { get; set; }
		public Nullable<short> Active { get; set; }
		public byte PageDefault { get; set; }
	}
}
