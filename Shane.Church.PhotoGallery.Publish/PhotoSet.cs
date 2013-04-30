using System;
using System.Collections.Generic;
using System.Text;

namespace Shane.Church.PhotoGallery.Publish
{
    internal class PhotoSet
    {
        public string Id;
        public string DisplayName;
        public string Title;
        public string Latitude;
        public string Longitude;


        /// <summary>
        /// Constructor insures the class is initialized properly.
        /// </summary>
        public PhotoSet()
        {
        }

        /// <summary>
        /// Constructor insures the class is initialized properly.
        /// </summary>
        public PhotoSet(string id, string display_name, string title, string lat, string lon)
        {
            this.Id = id;
            this.DisplayName = display_name;
            this.Title = title;
            this.Latitude = lat;
            this.Longitude = lon;
        }

        /// <summary>
        /// This function is used by the ComboBox control. When you add a 
        /// PhotoSet object to the ComboBox.Items collection this function 
        /// will be called to determine the text to be displayed.
        /// </summary>
        /// <returns>The string to be displayed in the ComboBox.</returns>
        public override string ToString()
        {
            return this.Title;
        }
    }
}
