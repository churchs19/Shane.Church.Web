using Shane.Church.Web.Cloud;
using Shane.Church.Web.Cloud.AWS;
using Shane.Church.Web.Cloud.Azure;
using Shane.Church.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateS3ToAzure
{
	class Program
	{
		static void Main(string[] args)
		{
			DataContext context = new DataContext();

			//photo.CategoryID.ToLower() + "." + photo.File.Substring(photo.File.LastIndexOf('/') + 1).ToLower()

			foreach(Photo p in context.Photos)
			{
				try
				{
					AWSS3BlobStorage amazon = new AWSS3BlobStorage();
					AzureBlobStorage azure = new AzureBlobStorage();

					if (!azure.ObjectExists(p.File))
					{
						MemoryStream ms = new MemoryStream();
						using (Stream s = amazon.ReadObject(p.PageId.ToLower() + "." + p.File.Substring(p.File.LastIndexOf("/") + 1).ToLower()))
						{
							s.CopyTo(ms);
						}
						if (ms.Length > 0)
						{
							ms.Seek(0, SeekOrigin.Begin);
							azure.WriteObject(p.File, ms);
						}
						else
						{
							Console.WriteLine(String.Format("FAILED {0} {1} to  copy to Azure", p.PhotoId, p.File));
						}
						ms.Close();
						Console.WriteLine(String.Format("Copied {0} {1} to Azure", p.PhotoId, p.File));
					}
					else
					{
						Console.WriteLine(String.Format("Skipping {0} {1}", p.PhotoId, p.File));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(String.Format("FAILED {0} {1} to copy to Azure - {2}", p.PhotoId, p.File, ex.Message));
				}
			}
			Console.ReadLine();
		}
	}
}
