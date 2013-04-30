using System;
using System.Collections.Generic;
using System.Text;

namespace Shane.Church.PhotoGallery.Publish
{
    internal class PhotoAttributes
    {
        public string Id;
        public string FullFilePath;
        public string Filename;
        public string Title;
        public string Description;
        public List<string> Tags;

        /// <summary>
        /// Constructor insures the class is initialized properly.
        /// </summary>
        public PhotoAttributes()
        {
            this.Tags = new List<string>();
        }

        /// <summary>
        /// Constructor insures the class is initialized properly.
        /// </summary>
        public PhotoAttributes(string id, string fullFilePath, string filename, string title, string description, List<string> tags)
        {
            this.Id = id;
            this.FullFilePath = fullFilePath;
            this.Filename = filename;
            this.Title = title;
            this.Description = description;
            this.Tags = tags;

        }
    }
}
