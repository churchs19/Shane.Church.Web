using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Shane.Church.Web.Cloud.AWS
{
	public class AWSS3BlobStorage : IBlobStorage
	{
		private AmazonS3 _amazonClient;
		private const string _containerPrefix = "shane.church.";
		private const string _containerName = "photos";

		public AWSS3BlobStorage()
		{
			_amazonClient = Amazon.AWSClientFactory.CreateAmazonS3Client("AKIAIMOJYLXER653ZADQ", "n29gwr97OgbePyrS14q8OVwhYt18S+DpVGipOpAm");
		}

		public void WriteObject(string objectName, System.IO.Stream content)
		{
			try
			{
				PutObjectRequest request = new PutObjectRequest();
				request.WithBucketName(_containerPrefix + _containerName).WithKey(objectName).WithInputStream(content);
				request.Timeout = 30000;
				S3Response response = _amazonClient.PutObject(request);
				response.Dispose();
			}
			catch (AmazonS3Exception amazonS3Exception)
			{
				//if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
				//{
				//    Console.WriteLine("Please check the provided AWS Credentials.");
				//    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
				//}
				//else
				//{
				//    Console.WriteLine("An Error, number {0}, occurred when creating a container with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
				//}
				throw amazonS3Exception;
			}
		}

		public System.IO.Stream ReadObject(string objectName)
		{
			try
			{
				GetObjectRequest request = new GetObjectRequest().WithBucketName(_containerPrefix + _containerName).WithKey(objectName);
				request.Timeout = 10000;
				GetObjectResponse response = _amazonClient.GetObject(request);
				return response.ResponseStream;
			}
			catch (AmazonS3Exception amazonS3Exception)
			{
				//if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
				//{
				//    Console.WriteLine("Please check the provided AWS Credentials.");
				//    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
				//}
				//else
				//{
				//    Console.WriteLine("An Error, number {0}, occurred when creating a container with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
				//}
				throw amazonS3Exception;
			}
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
			try
			{
				DeleteObjectRequest request = new DeleteObjectRequest();
				request.WithBucketName(_containerPrefix + _containerName)
					.WithKey(objectName);
				DeleteObjectResponse response = _amazonClient.DeleteObject(request);
			}
			catch (AmazonS3Exception amazonS3Exception)
			{
				//if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
				//{
				//    Console.WriteLine("Please check the provided AWS Credentials.");
				//    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
				//}
				//else
				//{
				//    Console.WriteLine("An Error, number {0}, occurred when creating a container with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
				//}
				throw amazonS3Exception;
			}
		}

		public List<string> ListObjects()
		{
			List<string> objects = new List<string>();
			try
			{
				ListObjectsRequest request = new ListObjectsRequest();
				request.BucketName = _containerPrefix + _containerName;
				using (ListObjectsResponse response = _amazonClient.ListObjects(request))
				{
					foreach (S3Object entry in response.S3Objects)
					{
						objects.Add(entry.Key);
					}
				}
			}
			catch (AmazonS3Exception amazonS3Exception)
			{
				//if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
				//{
				//    Console.WriteLine("Please check the provided AWS Credentials.");
				//    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
				//}
				//else
				//{
				//    Console.WriteLine("An Error, number {0}, occurred when creating a container with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
				//}
				throw amazonS3Exception;
			}
			return objects;
		}
	}
}
