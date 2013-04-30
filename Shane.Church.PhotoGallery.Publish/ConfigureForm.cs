using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using Windows.Live.SDK.Samples;
using Shane.Church.Web.Common.DataTransfer;
using Shane.Church.Web.Common;
using System.Linq;
using RestSharp;

namespace Shane.Church.PhotoGallery.Publish
{
	public partial class ConfigureForm : Form
	{
		/// <summary>
		/// This is the main entry point called from PublishPlugin.LaunchPluginConfig.
		/// </summary>
		/// <param name="parent">The window to be parented to.</param>
		/// <param name="sessionXml">The XmlDocument containing session specific information for this publish session.</param>
		/// <param name="persistXml">The XmlDocument containing persistent information for this plugin.</param>
		/// <returns>True if the user clicked Publish, otherwise false.</returns>
		public static bool GetSessionInfo(IWin32Window parent, XmlDocument sessionXml, XmlDocument persistXml)
		{
			ConfigureForm pf = new ConfigureForm();
			pf.sessionXml = sessionXml;
			pf.persistXml = persistXml;
			return pf.ShowDialog(parent) == DialogResult.OK;
		}

		private XmlDocument sessionXml;
		private XmlDocument persistXml;

		private string baseUri = "";
	   
		private static readonly string applicationThreadException = "A problem has occurred in this application:\r\n\r\n\t{0}.";
		private static readonly string applicationThreadExceptionTitle = "Unexpected error";

		/// <summary>
		/// This is the forms constructor, marked private so you must use GetSessionInfo above.
		/// </summary>
		private ConfigureForm()
		{
			// Establish an exception handler for this class.
			Application.ThreadException += new ThreadExceptionEventHandler(ConfigurationSettings_ThreadException);

			InitializeComponent();

			labelVersion.Text = "Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			this.comboBoxServer.SelectedIndex = 0;
			this.baseUri = Config.WebUriBase;

			PopulatePages();
		}

		private void PopulatePages()
		{
			comboBoxPages.Items.Clear();

			//HttpClient client = new HttpClient();
			//HttpResponseMessage message = AsyncHelpers.RunSync<HttpResponseMessage>(() => client.GetAsync(baseUri + "api/photocategoryapi/"));
			//IQueryable<PhotoCategoryDTO> categories = AsyncHelpers.RunSync<IQueryable<PhotoCategoryDTO>>(() => message.Content.ReadAsAsync<IQueryable<PhotoCategoryDTO>>());

			//var categoryNames = (from c in categories
			//					 select c.PageId).ToArray();
			//comboBoxPages.Items.AddRange(categoryNames);
			RestClient client = new RestClient(baseUri);
			RestRequest request = new RestRequest() { Method = Method.GET, Resource = "api/photocategoryapi/", RequestFormat = DataFormat.Json };
			IRestResponse<List<PhotoCategoryDTO>> response = client.Get<List<PhotoCategoryDTO>>(request);

			var categoryNames = (from c in response.Data
								 where c.PageId != null
								 select c.PageId);
			if (categoryNames.Any())
			{
				comboBoxPages.Items.AddRange(categoryNames.ToArray());
			}
		}

		/// <summary>
		/// Handles any unhandled exceptions thrown by the ConfigureForm by popping a dialog with the
		/// error message and then closing ConfigureForm.
		/// </summary>
		private void ConfigurationSettings_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(string.Format(applicationThreadException, e.Exception.Message),
							applicationThreadExceptionTitle,
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);

			// Fake that the user hit cancel and close ConfigureForm.
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void comboBoxServer_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxServer.SelectedItem.Equals("Local"))
				baseUri = Config.LocalUriBase;
			else
				baseUri = Config.WebUriBase;

			PopulatePages();
		}

		private void btnPublish_Click(object sender, EventArgs e)
		{
			PhotoSet ps = GetSelectedPhotoSet();
			XmlHelper.SessionRemoveUsePhotoSet(sessionXml);
			XmlHelper.SessionStoreUsePhotoSet(sessionXml, ps.Id, ps.Title, ps.DisplayName, ps.Latitude, ps.Longitude);
			XmlHelper.SessionStoreConnectionString(sessionXml, comboBoxServer.SelectedItem.ToString());
			XmlHelper.SessionStoreDimensions(sessionXml, int.Parse(textBoxMaxPreviewWidth.Text) , int.Parse(textBoxMaxPreviewHeight.Text), int.Parse(textBoxResizeSize.Text));
			XmlHelper.SessionFlagVideos(sessionXml);
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private PhotoSet GetSelectedPhotoSet()
		{
			PhotoSet ps = new PhotoSet();

			try
			{

				string page = comboBoxPages.SelectedItem.ToString();

				//HttpClient client = new HttpClient();
				//HttpResponseMessage message = AsyncHelpers.RunSync<HttpResponseMessage>(() => client.GetAsync(baseUri + "api/photocategoryapi/" + page));
				//PhotoCategoryDTO category = AsyncHelpers.RunSync<PhotoCategoryDTO>(() => message.Content.ReadAsAsync<PhotoCategoryDTO>());

				RestClient client = new RestClient(baseUri);
				RestRequest request = new RestRequest() { Resource = "api/photocategoryapi/" + Uri.EscapeDataString(page), RequestFormat = DataFormat.Json, Method = Method.GET };
				IRestResponse<PhotoCategoryDTO> response = client.Get<PhotoCategoryDTO>(request);

				PhotoCategoryDTO category = response.Data;

				ps.Id = category.PageId;
				ps.DisplayName = category.DisplayName;
				ps.Title = category.PageTitle;
				ps.Latitude = category.Latitude;
				ps.Longitude = category.Longitude;
			}
			catch
			{

			}
			finally
			{
			}

			return ps;
		}

		private void buttonCreateNew_Click(object sender, EventArgs e)
		{
			if (new AddPage(baseUri).ShowDialog().Equals(DialogResult.OK))
			{
				PopulatePages();
			}
		}
	}
}
