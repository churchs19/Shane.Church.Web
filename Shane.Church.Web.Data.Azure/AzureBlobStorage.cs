using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Shane.Church.Web.Cloud.Azure
{
	public class AzureBlobStorage : IBlobStorage
	{
		CloudBlobClient _blobClient;

		public AzureBlobStorage()
		{
			// Retrieve storage account from connection string.
			CloudStorageAccount storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.StorageCredentialsAccountAndKey("schurchweb", "qSABklIU9GOHwfvxJ8OOqN5WYLukIRFwHqEtzs0M6aWULHUg4yf17n7XZqOX5Ip4M+kwyMTAxA7X+HcDYM8b0Q=="), true);

			// Create the blob client.
			_blobClient = storageAccount.CreateCloudBlobClient();
		}

		private CloudBlobContainer GetContainer()
		{
			// Retrieve a reference to a container. 
			CloudBlobContainer container = _blobClient.GetContainerReference("schurchweb");

			// Create the container if it doesn't already exist.
			container.CreateIfNotExist();

			return container;
		}

		public void WriteObject(string objectName, System.IO.Stream content)
		{
			CloudBlockBlob blockBlob = GetContainer().GetBlockBlobReference(objectName);

			// Create or overwrite the "myblob" blob with contents from a local file.
			blockBlob.UploadFromStream(content);
		}

		public System.IO.Stream ReadObject(string objectName)
		{
			MemoryStream ms = new MemoryStream();
			CloudBlockBlob blockBlob = GetContainer().GetBlockBlobReference(objectName);

			blockBlob.DownloadToStream(ms);
			ms.Seek(0, SeekOrigin.Begin);
			return ms;
		}

		public Bitmap ReadImage(string objectName)
		{
			using (Stream stream = ReadObject(objectName))
			{
				Bitmap bmp = new Bitmap(stream);
				return bmp;
			}
		}

		public void DeleteObject(string objectName)
		{
			CloudBlockBlob blob = GetContainer().GetBlockBlobReference(objectName);
			blob.DeleteIfExists();
		}

		public List<string> ListObjects()
		{
			List<string> objects = new List<string>();
			IEnumerable<IListBlobItem> blobs = GetContainer().ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });
			foreach (var b in blobs)
				objects.Add(b.Uri.ToString());
			return objects;
		}

		public bool ObjectExists(string objectName)
		{
			CloudBlockBlob blob = GetContainer().GetBlockBlobReference(objectName);
			return blob != null;
		}
	}
}
