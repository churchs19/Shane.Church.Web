using System;
using System.Collections.Generic;

namespace Shane.Church.Web.Data.Models
{
    public class Book
    {
        public long BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string PurchaseHtml { get; set; }
        public string ImageHtml { get; set; }
        public Nullable<short> Active { get; set; }
        public string Comments { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}
