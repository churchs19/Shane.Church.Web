using System;
using System.Linq;
using XmlRpcMvc;
using XmlRpcMvc.Common;
using Shane.Church.Web.v2012.XmlRpc.Models;
using System.IO;
using Shane.Church.Web.Data.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace Shane.Church.Web.v2012.XmlRpc
{
	public class MetaWeblogApi : IXmlRpcService
	{
		[XmlRpcMethod("metaWeblog.newPost")]
		public string NewPost(string blogid, string username, string password, PostInfo post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				string id = string.Empty;

				if (post.DateCreated == DateTime.MinValue)
					post.DateCreated = DateTime.Now;

				DataContext model = new DataContext();

				Journal newEntry = new Journal();
				newEntry.Entry = post.Description;
				newEntry.EntryDate = post.DateCreated;
				newEntry.Title = post.Title;
				newEntry.User = username;

				foreach (string tag in post.Categories)
				{
					JournalTags t = new JournalTags();
					t.Journal = newEntry;
					t.Tag = tag;
					model.JournalTags.Add(t);
				}

				model.Journals.Add(newEntry);
				model.SaveChanges();

				id = newEntry.Id.ToString();

				return id;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("metaWeblog.editPost")]
		public bool EditPost(string postid, string username, string password, PostInfo post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				bool result = false;

				if (post.DateCreated == DateTime.MinValue)
					post.DateCreated = DateTime.Now;

				try
				{
					DataContext model = new DataContext();

					long id = Convert.ToInt64(postid);

					var entryQuery = (from e in model.Journals
									  where e.Id == id
									  select e);
					if (entryQuery.Count() > 0)
					{
						Journal entry = entryQuery.First();
						entry.Entry = post.Description;
						entry.EntryDate = post.DateCreated;
						entry.Title = post.Title;
						entry.User = username;

						foreach(var jt in entry.JournalTags)
						{
							model.JournalTags.Remove(jt);
						}
						foreach (string tag in post.Categories)
						{
							JournalTags t = new JournalTags();
							t.Tag = tag;
							t.Journal = entry;
							model.JournalTags.Add(t);
						}

						model.SaveChanges();
						result = true;
					}

				}
				catch { result = false; }

				return result;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("metaWeblog.getPost")]
		public PostInfo GetPost(string postid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				PostInfo post = new PostInfo();

				DataContext model = new DataContext();

				long id = long.Parse(postid);

				var entryQuery = (from e in model.Journals
								  where e.Id == id
								  select e);
				if (entryQuery.Count() > 0)
				{
					Journal entry = entryQuery.First();

					post.DateCreated = entry.EntryDate;
					post.Description = entry.Entry;
					post.Title = entry.Title;
					post.Categories = (from t in entry.JournalTags select t.Tag).ToArray();
				}

				return post;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("metaWeblog.getCategories")]
		public CategoryInfo[] GetCategories(string blogid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

				DataContext model = new DataContext();

				var tags = (from t in model.JournalTags
							select t.Tag).Distinct();
				foreach (string tag in tags)
				{
					CategoryInfo info = new CategoryInfo();
					info.Title = tag;
					info.Description = tag;
					categoryInfos.Add(info);
				}

				return categoryInfos.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("metaWeblog.getRecentPosts")]
		public PostInfo[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
		{
			if (ValidateUser(username, password))
			{
				DataContext model = new DataContext();

				var posts = (from j in model.Journals
							 orderby j.EntryDate descending
							 select j).Take(numberOfPosts).ToList();

				List<PostInfo> postList = new List<PostInfo>();

				foreach (Journal j in posts)
				{
					postList.Add(new PostInfo()
					{
						DateCreated = j.EntryDate,
						Description = j.Entry,
						Title = j.Title,
						Categories = (from t in j.JournalTags select t.Tag).ToArray(),
					});
				}

				return postList.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("metaWeblog.newMediaObject")]
		public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
		{
			if (ValidateUser(username, password))
			{
				MediaObjectInfo objectInfo = new MediaObjectInfo();

				try
				{
					Guid fileGuid = Guid.NewGuid();
					string strDir = ConfigurationManager.AppSettings["JournalImages"];
					string strPath = HttpContext.Current.Server.MapPath(strDir);
					string strFileName = "";

					switch (mediaObject.TypeName)
					{
						case "image/jpeg":
							strFileName = fileGuid.ToString() + ".jpg";
							break;
						case "image/gif":
							strFileName = fileGuid.ToString() + ".gif";
							break;
						case "image/png":
							strFileName = fileGuid.ToString() + ".png";
							break;
						default:
							throw new Exception("File format not supported");
					}

					BuildFolders(strPath, mediaObject.Name);
					String strFullFileName = strPath + Path.DirectorySeparatorChar + mediaObject.Name;
					File.WriteAllBytes(strFullFileName, mediaObject.Bits);

					objectInfo.Url = "http://www.s-church.net/journal_images/" + mediaObject.Name;
				}
				catch (Exception ex)
				{
					throw new XmlRpcFaultException(0, ex.Message);
				}

				return objectInfo;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("blogger.deletePost")]
		public bool DeletePost(string key, string postid, string username, string password, bool publish)
		{
			if (ValidateUser(username, password))
			{
				bool result = false;

				try
				{
					DataContext model = new DataContext();

					long id = long.Parse(postid);

					var commentsQuery = (from c in model.JournalComments
										 where c.JournalId == id
										 select c);
					foreach (JournalComments c in commentsQuery)
					{
						model.JournalComments.Remove(c);
					}

					var entryQuery = (from e in model.Journals
									  where e.Id == id
									  select e);
					if (entryQuery.Count() > 0)
					{
						model.Journals.Remove(entryQuery.First());
					}

					model.SaveChanges();
					result = true;
				}
				catch { result = false; }

				return result;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("blogger.getUsersBlogs")]
		public BlogInfo[] GetUsersBlogs(string key, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				List<BlogInfo> infoList = new List<BlogInfo>();

				BlogInfo bi = new BlogInfo();
				bi.Blogid = "http://www.s-church.net";
				bi.BlogName = "Shane Church - Blog";
				bi.Url = "http://www.s-church.net";

				infoList.Add(bi);

				return infoList.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		[XmlRpcMethod("blogger.getUserInfo")]
		public UserInfo GetUserInfo(string key, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				UserInfo info = new UserInfo();

				info.Email = username.ToLower() + "@s-church.net";
				info.FirstName = username;
				info.LastName = "";
				info.Url = "http://www.s-church.net";
				info.UserId = username.ToLower();
				info.NickName = "";

				return info;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		#region Private Methods
		private bool ValidateUser(string username, string password)
		{
			return (username == "shane") && (password == "PasadenA!");
		}

		private void BuildFolders(string basePath, string fileName)
		{
			string[] parts = fileName.Split(new char[] { '/', '\\' });
			string strPath = basePath;
			for (int i = 0; i < parts.Length - 1; i++)
			{
				strPath += Path.DirectorySeparatorChar + parts[i];
				if (!Directory.Exists(strPath))
				{
					Directory.CreateDirectory(strPath);
				}
			}
		}
		#endregion

	}
}