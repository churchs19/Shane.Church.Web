using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shane.Church.Web.v2012.Models.Software
{
	public class SoftwareViewModel
	{
		public SoftwareViewModel()
		{
			Id = 1;
		}

		public SoftwareViewModel(int id)
		{
			Id = id;
		}

		public int Id { get; set; }
	}
}