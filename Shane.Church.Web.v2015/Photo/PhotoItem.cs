using System;
using Newtonsoft.Json;

namespace Shane.Church.Web.Photo
{
	public enum PhotoType
	{
		Local,
		Instagram
	}

    public class PhotoItem
    {
		public string Id { get; set; }
		public PhotoType PhotoType { get; set; }
		public string SmallImage { get; set; }
		public string LargeImage { get; set; }
		public string Caption { get; set; }
    }
}