using System;
using System.Collections.Generic;

namespace Shane.Church.Web.Photo.Instagram.Models
{
	public class MediaResponse
	{
		public Pagination pagination { get; set; }
		public Meta meta { get; set; }
		public List<Media> data { get; set; }
	}

	public class Pagination
	{
		public string next_url { get; set; }
		public string next_max_id { get; set; }
	}

	public class Meta
	{
		public int code { get; set; }
	}

	public class Media
	{
		public string attribution { get; set; }
		public List<string> tags { get; set; }
		public Location location { get; set; }
		public Comments comments { get; set; }
		public string filter { get; set; }
		public string created_time { get; set; }
		public string link { get; set; }
		public Likes likes { get; set; }
		public Images images { get; set; }
		public List<UserInPhoto> users_in_photo { get; set; }
		public Caption caption { get; set; }
		public string type { get; set; }
		public string id { get; set; }
		public UserInfo user { get; set; }
	}

	public class Location
	{
		public float latitude { get; set; }
		public string name { get; set; }
		public float longitude { get; set; }
		public int id { get; set; }
	}

	public class Comments
	{
		public int count { get; set; }
		public CommentData[] data { get; set; }
	}

	public class CommentData
	{
		public string created_time { get; set; }
		public string text { get; set; }
		public UserInfo from { get; set; }
		public string id { get; set; }
	}

	public class UserInfo
	{
		public string username { get; set; }
		public string profile_picture { get; set; }
		public string id { get; set; }
		public string full_name { get; set; }
	}

	public class Likes
	{
		public int count { get; set; }
		public UserInfo[] data { get; set; }
	}

	public class Images
	{
		public ImageData low_resolution { get; set; }
		public ImageData thumbnail { get; set; }
		public ImageData standard_resolution { get; set; }
	}

	public class ImageData
	{
		public string url { get; set; }
		public int width { get; set; }
		public int height { get; set; }
	}

	public class Caption
	{
		public string created_time { get; set; }
		public string text { get; set; }
		public UserInfo from { get; set; }
		public string id { get; set; }
	}

	public class UserInPhoto
	{
		public Position position { get; set; }
		public UserInfo user { get; set; }
	}

	public class Position
	{
		public float x { get; set; }
		public float y { get; set; }
	}
}