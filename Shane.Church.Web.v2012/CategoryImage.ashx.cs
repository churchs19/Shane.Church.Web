using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Shane.Church.Web.Data.Models;
using System.IO;
using Shane.Church.Web.v2012.Helpers;
using Shane.Church.Web.Data;

namespace Shane.Church.Web.v2012
{
	/// <summary>
	/// Summary description for CategoryImage
	/// </summary>
	public class CategoryImage : IHttpHandler
	{
		private IBlobStorage _storageController;

		public CategoryImage(IBlobStorage storage)
		{
			if (storage == null)
				throw new ArgumentNullException("storage"); 
			_storageController = storage;
		}

		public void ProcessRequest(HttpContext context)
		{
//			int size = 200;
			string CategoryId = context.Request.Params["PAGE"];
			float maxSize = 200f;
			float minSize = 0f;
			bool square = false;
			if (!string.IsNullOrEmpty(context.Request.Params["MIN_SIZE"]))
			{
				try
				{
					minSize = float.Parse(context.Request.Params["MIN_SIZE"]);
				}
				catch { minSize = 200f; }
			}
			if (!string.IsNullOrEmpty(context.Request.Params["SQUARE"]))
			{
				if (context.Request.Params["SQUARE"].ToUpper().StartsWith("Y"))
				{
					square = true;
				}
			}

			Bitmap bmp = null;
			context.Response.ContentType = "image/png";
			try
			{
				if (CategoryId != null)
				{
//					bmp = CreateFolderImage(CategoryId, size, context);

					DataContext model = new DataContext();
					var photo = (from p in model.Photos
								 where p.PageId == CategoryId
								 orderby p.PageDefault descending, p.PhotoId descending
								 select p).FirstOrDefault();

					bmp = _storageController.ReadImage(photo.File);

					float percent = 100.00f;

					if (minSize > 0)
					{
						if (bmp.Height < bmp.Width)
						{
							percent = minSize / (float)bmp.Height;
						}
						else
						{
							percent = minSize / (float)bmp.Width;
						}
					}
					else
					{
						if (bmp.Height > bmp.Width)
						{
							percent = maxSize / (float)bmp.Height;
						}
						else
						{
							percent = maxSize / (float)bmp.Width;
						}
					}

					if (!square)
					{
						bmp.ScaleByPercent(percent).Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
					}
					else
					{
						bmp.ScaleByPercent(percent).SquareCropImage().Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
					}
				}
				else
				{
					context.Response.StatusCode = 404;
				}
			}
			catch
			{
				context.Response.StatusCode = 500;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Creates the thumbnail Bitmap for a folder.
		/// </summary>
		/// <param name="CategoryId">The image category to build</param>
		/// <param name="size">The size in pixels of a square bounding the thumbnail.</param>
		/// <returns>The thumbnail Bitmap.</returns>
		Bitmap CreateFolderImage(string CategoryId, int size, HttpContext context)
		{
			int UpFolderStackHeight = 6;
			Color UpFolderBorderColor = Color.FromArgb(0x00, 0xC0, 0x00);
			int UpFolderBorderWidth = 2;

			Bitmap newImage = new Bitmap(size, size);
			Graphics g = Graphics.FromImage(newImage);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			g.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), 0, 0, size, size);

			Random rnd = new Random();
			List<Photo> imagesToDraw = new List<Photo>();
			DataContext model = new DataContext();
			var defaultPhotos = (from p in model.Photos
								 where p.PageDefault > 0 && p.Active.HasValue && p.Active.Value == -1 && p.PageId == CategoryId
								 select p);
			imagesToDraw.AddRange(defaultPhotos);
			if (imagesToDraw.Count < UpFolderStackHeight)
			{
				//DataSet dsPhotos = pds.GetNewestXPhotosByPageMinusDefaultPhotos(CategoryId, UpFolderStackHeight - imagesToDraw.Count, SortDirection.Descending);
				var photos = (from p in model.Photos
							  where p.Active.HasValue && p.Active == -1 && p.PageId == CategoryId
							  orderby p.UpdatedDate descending
							  select p).Take(UpFolderStackHeight - imagesToDraw.Count);
				imagesToDraw.AddRange(photos);
			}
			int drawXOffset = size / 2;
			int drawYOffset = size / 2;
			double angleAmplitude = Math.PI / 10;
			int imageFolderSize = (int)(size / (Math.Cos(angleAmplitude) + Math.Sin(angleAmplitude)));

			for (int i = imagesToDraw.Count - 1; i >= 0; i--)
			{
				try
				{
					Stream data = _storageController.ReadObject(imagesToDraw[i].File);
					Bitmap folderImage = new Bitmap(data);
					data.Close();

					int width = folderImage.Width;
					int height = folderImage.Height;
					if (imageFolderSize > 0 && folderImage.Width >= folderImage.Height && folderImage.Width > imageFolderSize)
					{
						width = imageFolderSize;
						height = imageFolderSize * folderImage.Height / folderImage.Width;
					}
					else if (imageFolderSize > 0 && folderImage.Height >= folderImage.Width && folderImage.Height > imageFolderSize)
					{
						width = imageFolderSize * folderImage.Width / folderImage.Height;
						height = imageFolderSize;
					}

					Pen UpFolderBorderPen = new Pen(new SolidBrush(UpFolderBorderColor), UpFolderBorderWidth);
					UpFolderBorderPen.LineJoin = LineJoin.Round;
					UpFolderBorderPen.StartCap = LineCap.Round;
					UpFolderBorderPen.EndCap = LineCap.Round;

					double angle = (0.5 - rnd.NextDouble()) * angleAmplitude;
					if (i == 0)
					{
						angle = 0.0;
					}
					float sin = (float)Math.Sin(angle);
					float cos = (float)Math.Cos(angle);
					float sh = sin * height / 2;
					float ch = cos * height / 2;
					float sw = sin * width / 2;
					float cw = cos * width / 2;
					float shb = sin * (height + UpFolderBorderPen.Width) / 2;
					float chb = cos * (height + UpFolderBorderPen.Width) / 2;
					float swb = sin * (width + UpFolderBorderPen.Width) / 2;
					float cwb = cos * (width + UpFolderBorderPen.Width) / 2;

					g.DrawPolygon(UpFolderBorderPen, new PointF[] {
				   new PointF(
					   (float)drawXOffset - cwb - shb,
					   (float)drawYOffset + chb - swb),
				   new PointF(
					   (float)drawXOffset - cwb + shb,
					   (float)drawYOffset - swb - chb),
				   new PointF(
					   (float)drawXOffset + cwb + shb,
					   (float)drawYOffset + swb - chb),
				   new PointF(
					   (float)drawXOffset + cwb - shb,
					   (float)drawYOffset + swb + chb)
						});

					g.DrawImage(folderImage, new PointF[] {
				   new PointF(
					   (float)drawXOffset - cw + sh,
					   (float)drawYOffset - sw - ch),
				   new PointF(
					   (float)drawXOffset + cw + sh,
					   (float)drawYOffset + sw - ch),
				   new PointF(
					   (float)drawXOffset - cw - sh,
					   (float)drawYOffset + ch - sw)
						});
					folderImage.Dispose();
				}
				catch
				{
				
				}
			}
			
			g.Dispose();
			return newImage;
		}
	}
}