using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shane.Church.Web.v2012.Models
{
	public class PhotoViewModel
	{
		public PhotoViewModel()
		{

		}

		public long ID { get; set; }
		public string CategoryID { get; set; }
		public string CategoryName { get; set; }
		public string Caption { get; set; }
		public string File { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}