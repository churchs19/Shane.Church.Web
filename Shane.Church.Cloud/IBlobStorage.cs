using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Shane.Church.Web.Cloud
{
	public interface IBlobStorage
	{
		void WriteObject(string keyName, Stream content);
		Stream ReadObject(string objectName);
		Bitmap ReadImage(string objectName);
		void DeleteObject(string objectName);
		List<string> ListObjects();
	}
}
