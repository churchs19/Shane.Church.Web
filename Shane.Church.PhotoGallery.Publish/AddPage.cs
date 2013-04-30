using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Shane.Church.Web.Common;
using Shane.Church.Web.Common.DataTransfer;
using RestSharp;

namespace Shane.Church.PhotoGallery.Publish
{
    public partial class AddPage : Form
    {
		private string baseUri = "";

        public AddPage(string sUriBase)
        {
            InitializeComponent();
			baseUri = sUriBase;
            vEarthControl1.ShowInitialMap(cs_Andr.Controls.VirtualEarth.VEarthControl.SDKVersion.Version6);
            this.textBoxLatitude.Text = vEarthControl1.getLat();
            this.textBoxLongitude.Text = vEarthControl1.getLon();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string ID=textBoxID.Text.Trim().ToUpper();
            string DisplayName=textBoxDisplayName.Text.Trim();
            string Title=textBoxPageTitle.Text.Trim();
            string Latitude=textBoxLatitude.Text.Trim();
            string Longitude=textBoxLongitude.Text.Trim();

            if (InsertPage(ID, DisplayName, Title, 0, -1, Latitude, Longitude))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                MessageBox.Show("Error creating new page.");
            }
        }

        private void AddPage_Load(object sender, EventArgs e)
        {
            vEarthControl1.Focus();
        }

        private void vEarthControl1_OnClickOnMap(object sender, cs_Andr.Controls.VirtualEarth.OnClickOnMapEventArgs e)
        {
            this.textBoxLatitude.Text = String.Format("{0:0.000000;-0.000000;0.000000}", e.Lat);
            this.textBoxLongitude.Text = String.Format("{0:0.000000;-0.000000;0.000000}", e.Lon);
        }

        private void vEarthControl1_OnMoveOnMap(object sender, cs_Andr.Controls.VirtualEarth.OnMoveOnMapEventArgs e)
        {

        }

        private void AddPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            vEarthControl1.unInit();
        }

        private bool InsertPage(string ID, string DISPLAY_NAME, string PAGE_TITLE, int SORT_ORDER, short ACTIVE, string LATITUDE, string LONGITUDE)
        {
			try
			{
				PhotoCategoryDTO dto = new PhotoCategoryDTO()
				{
					PageId = ID,
					DisplayName = DISPLAY_NAME,
					PageTitle = PAGE_TITLE,
					SortOrder = SORT_ORDER,
					Active = ACTIVE,
					Latitude = LATITUDE,
					Longitude = LONGITUDE
				};
				RestClient client = new RestClient(baseUri);
				RestRequest request = new RestRequest() { Resource = "api/photocategoryapi/", Method = Method.POST, RequestFormat = DataFormat.Json };
				request.AddBody(dto);
				IRestResponse response = client.Post(request);

				return true;
			}
			catch
			{
				return false;
			}
        }

    }
}
