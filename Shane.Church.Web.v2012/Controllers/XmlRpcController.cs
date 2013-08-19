using Shane.Church.Web.v2012.XmlRpc;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using XmlRpcMvc;

namespace Shane.Church.Web.v2012.Controllers
{
	public class XmlRpcController : Controller
	{
		public ActionResult Rsd()
		{
			var ms = new MemoryStream();
			var xmlWriter = XmlWriter.Create(ms);

			xmlWriter.WriteStartDocument();
			{
				xmlWriter.WriteStartElement("rsd", "http://archipelago.phrasewise.com/rsd");
				xmlWriter.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
				xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
				xmlWriter.WriteAttributeString("version", "1.0");
				{
					xmlWriter.WriteStartElement("service", string.Empty);
					{
						xmlWriter.WriteStartElement("engineName");
						xmlWriter.WriteString("~ ENGINE NAME ~");
						xmlWriter.WriteEndElement();

						xmlWriter.WriteStartElement("homePageLink");
						xmlWriter.WriteString(Url.Action("Index", "Home"));
						xmlWriter.WriteEndElement();

						xmlWriter.WriteStartElement("apis");
						{
							xmlWriter.WriteStartElement("api");
							xmlWriter.WriteAttributeString("name", "MetaWeblog");
							xmlWriter.WriteAttributeString("preferred", "true");
							xmlWriter.WriteAttributeString("apiLink", Url.Action("Endpoint", "XmlRpc"));
							xmlWriter.WriteAttributeString("blogID", "123");
							xmlWriter.WriteEndElement();
						}
						xmlWriter.WriteEndElement();

					}
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndDocument();

			xmlWriter.Flush();
			ms.Position = 0;

			return new FileStreamResult(ms, "text/xml");
		}

		public ActionResult Endpoint()
		{
			return new XmlRpcResult(typeof(MetaWeblogApi));
		}
	}
}
