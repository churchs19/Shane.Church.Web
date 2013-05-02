using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shane.Church.Web.v2012.Models.Blog
{
	public class ArchiveLinkModel
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public string MonthName
		{
			get
			{
				return new DateTime(Year, Month, 1).ToString("MMMM");
			}
		}
	}
}