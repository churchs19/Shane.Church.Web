using System.Runtime.Serialization;

namespace Shane.Church.Web.v2012.XmlRpc.Models
{
    public class MediaObjectInfo
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
