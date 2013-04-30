using System;
using System.Collections.Generic;

namespace Shane.Church.Web.Data.Models
{
    public class JournalComments
    {
        public long CommentId { get; set; }
        public Nullable<long> JournalId { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public string Comments { get; set; }
        public string IPAddress { get; set; }
        public virtual Journal Journal { get; set; }
    }
}
