using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsLive.PublishPlugins;
using System.Windows.Forms;
using Windows.Live.SDK.Samples;
using System.Drawing;
using System.IO;
using Shane.Church.Web.Cloud.Azure;
using Shane.Church.Web.Common.DataTransfer;
using Shane.Church.Web.Common;
using RestSharp;

namespace Shane.Church.PhotoGallery.Publish
{
	public class PublishPlugin: IPublishPlugin
	{
		private static readonly string uploadFailureLogFormat = "ERROR: {0} failed to upload with the following error:\r\n\t{1}";
		//private static readonly string uploadSuccessLogFormat = "{0} ({1}) uploaded successfully";
		//private static readonly string addPhotoSuccessLogFormat = "Added photo ({0}) to the photo set ({1})";
		//private static readonly string addPhotoFailureLogFormat = "ERROR: failed to add photo ({0}) to the photo set ({1}) with the following error:\r\n\t{2}";
		//private static readonly string createPhotoSetSuccessLogFormat = "Created the photo set {0}({1}) successfully";
		//private static readonly string createPhotoSetFailureLogFormat = "ERROR: failed to create photo set {0} with the following error:\r\n\t{1}";
		//private static readonly string userCanceledUploadMessage = "User canceled the upload.";
		private static readonly string uploadVideoFailureMessage = "User does not have permissions to upload videos.";

		private string sUriBase = "";

		#region IPublishPlugin Members

		public bool HasPublishResults(System.Xml.XmlDocument sessionXml)
		{
			return false;
		}

		public bool HasSummaryInformation(System.Xml.XmlDocument sessionXml)
		{
			return false;
		}

		public void LaunchPublishResults(System.Xml.XmlDocument sessionXml)
		{

		}

		public bool PublishItem(System.Windows.Forms.IWin32Window parentWindow, string mediaObjectId, System.IO.Stream stream, System.Xml.XmlDocument sessionXml, IPublishProperties publishProperties, IPublishProgressCallback callback, System.Threading.EventWaitHandle cancelEvent)
		{
			// Load media object specific settings.
			PhotoAttributes photoAttributes = new PhotoAttributes();
			if (XmlHelper.SessionLoadItemInfo(sessionXml, mediaObjectId, out photoAttributes.FullFilePath, out photoAttributes.Filename, out photoAttributes.Title, out photoAttributes.Tags) == false)
			{
				return false;
			}

			// Check if we set a flag to NOT upload this item.
			if (!XmlHelper.SessionShouldUploadItem(sessionXml, mediaObjectId))
			{
				XmlHelper.SessionLog(sessionXml, true, string.Format(uploadFailureLogFormat, photoAttributes.Filename, uploadVideoFailureMessage));
				return false;
			}

			PhotoSet ps = new PhotoSet();
			if (XmlHelper.SessionLoadUsePhotoSet(sessionXml, out ps.Id, out ps.Title, out ps.DisplayName, out ps.Latitude, out ps.Longitude))
			{
				try
				{
					string sConnectLocale = "";
					if (XmlHelper.SessionLoadConnectionString(sessionXml, out sConnectLocale))
					{
						if (sConnectLocale.Equals("Local"))
							sUriBase = Config.LocalUriBase;
						else
							sUriBase = Config.WebUriBase;
					}
					else
						return false;

					int maxWidth, maxHeight, resizeSize;
					if (!XmlHelper.SessionLoadDimensions(sessionXml, out maxWidth, out maxHeight, out resizeSize))
						return false;

					Image img = Image.FromFile(photoAttributes.FullFilePath);
					DateTime dateTaken;
					try
					{
						dateTaken = PhotoUtility.GetDateTaken(img);
					}
					catch
					{
						dateTaken = DateTime.Now;
					}
					Image resizeImg = PhotoUtility.Resize(img, resizeSize);
					int width, height;
					PhotoUtility.GetDisplaySizes(resizeImg, maxWidth, maxHeight, out width, out height);

					if (!sUriBase.Equals(Config.LocalUriBase))
					{
						UploadPhoto(ps.Id.ToLower(), photoAttributes.Filename, resizeImg);
					}

					long id = InsertPhoto(ps.Id, photoAttributes.Title, "photo/" + ps.Id.ToLower() + "/" + photoAttributes.Filename, width, height, 0, -1, 0, dateTaken);
					if (id == -1)
						return false;

					//if (!AttachTags(id, photoAttributes.Tags))
					//	return false;

					resizeImg.Dispose();
					img.Dispose();

					XmlHelper.SessionLog(sessionXml, false, photoAttributes.Filename + " published successfully.");
				}
				catch (Exception ex)
				{
					XmlHelper.SessionLog(sessionXml, true, ex.Message);
					return false;
				}
			}
			else
			{
				XmlHelper.SessionLog(sessionXml, true, "Unable to load Photo Set data");
				return false;
			}

			callback.SetPublishProgress(100);
			return true;
		}

		public bool ShowConfigurationSettings(System.Windows.Forms.IWin32Window parentWindow, System.Xml.XmlDocument sessionXml, System.Xml.XmlDocument persistXml, IPublishProperties publishProperties)
		{
			return ConfigureForm.GetSessionInfo(parentWindow, sessionXml, persistXml);
		}

		public void ShowSummaryInformation(System.Windows.Forms.IWin32Window parentWindow, System.Xml.XmlDocument sessionXml)
		{
		}

		#endregion

		private void UploadPhoto(string categoryId, string filename, Image img)
		{
			AzureBlobStorage storage = new AzureBlobStorage();
			using (MemoryStream imgStream = new MemoryStream())
			{
				img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg);
				imgStream.Seek(0, SeekOrigin.Begin);

				storage.WriteObject("photo/" + categoryId + "/" + filename, imgStream);
			}
		}

		private long InsertPhoto(string LINK_ID, string CAPTION, string FILE, int DISPLAY_WIDTH, int DISPLAY_HEIGHT, int SORT_ORDER, short ACTIVE, int PAGE_DEFAULT, DateTime UPDATED_DATE)
		{
			try
			{
				//HttpClient client = new HttpClient();
				//HttpResponseMessage message = await client.PostAsJsonAsync<PhotoDTO>("", null);
				//message.EnsureSuccessStatusCode();
				//long photoId = await message.Content.ReadAsAsync<long>();
				//return photoId;
				PhotoDTO photo = new PhotoDTO() {
					PageId = LINK_ID,
					Caption = CAPTION,
					File = FILE,
					DisplayWidth = DISPLAY_WIDTH,
					DisplayHeight = DISPLAY_HEIGHT,
					SortOrder = SORT_ORDER,
					Active = ACTIVE,
					PageDefault = (byte)PAGE_DEFAULT,
					UpdatedDate = UPDATED_DATE
				};
				RestClient client = new RestClient(sUriBase);
				RestRequest request = new RestRequest() { Resource = "api/photoapi/", RequestFormat= DataFormat.Json, Method = Method.POST };
				request.AddBody(photo);
				IRestResponse response = client.Post(request);
				return long.Parse(response.Content);
			}
			catch
			{
				return -1;
			}
		}

		//private bool AttachTags(long PhotoId, List<string> tags)
		//{
		//	foreach (string s in tags)
		//	{
		//		try
		//		{

		//		}
		//		catch { return false; }
		//		finally { }
		//	}
		//	return true;
		//}
	}
}
