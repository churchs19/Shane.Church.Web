using System;
using System.Collections.Generic;

namespace Shane.Church.Web.Data.Models
{
    public class Page
    {
        public Page()
        {
            this.Photos = new List<Photo>();
        }

        public string PageId { get; set; }
        public string DisplayName { get; set; }
        public string PageTitle { get; set; }
        public long SortOrder { get; set; }
        public Nullable<short> Active { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
