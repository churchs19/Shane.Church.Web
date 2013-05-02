using System.Runtime.Serialization;

namespace Shane.Church.Web.v2012.XmlRpc.Models
{
    public class CategoryInfo
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }
		
		[DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
