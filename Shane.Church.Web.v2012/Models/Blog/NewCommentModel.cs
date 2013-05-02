using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Shane.Church.Web.v2012.Models.Blog
{
	public class NewCommentModel
	{
		public NewCommentModel()
		{

		}

		[Required]
		[DisplayName("ID")]
		public long ID { get; set; }

		[Required]
		[DisplayName("Name")]
		public string UserName { get; set; }

		[Required]
		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[DisplayName("Link")]
		[DataType(DataType.Url)]
		public string Link { get; set; }
		
		[Required]
		[DisplayName("Comments")]
		public string Comments { get; set; }
	}
}