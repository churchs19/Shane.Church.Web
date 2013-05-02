using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;

namespace Shane.Church.Web.v2012.Helpers
{
	public static class Utility
	{
		/// <summary>
		/// Utility function to scale an image to the specified percentage of its original size
		/// </summary>
		/// <param name="Photo">The image to scale</param>
		/// <param name="Percent">The percentage of the original size to scale the photo to</param>
		/// <returns>The scaled photo</returns>
		public static Image ScaleByPercent(this Image Photo, float Percent)
		{
			int sourceWidth = Photo.Width;
			int sourceHeight = Photo.Height;
			int destWidth = (int)(sourceWidth * Percent);
			int destHeight = (int)(sourceHeight * Percent);

			Bitmap bmp = new Bitmap(destWidth, destHeight,
									 PixelFormat.Format24bppRgb);
			bmp.SetResolution(Photo.HorizontalResolution,
									Photo.VerticalResolution);

			Graphics g = Graphics.FromImage(bmp);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			g.DrawImage(Photo,
				new Rectangle(0, 0, destWidth, destHeight),
				new Rectangle(0, 0, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			g.Dispose();
			return bmp;
		}

		/// <summary>
		/// Utility function to square an image centering the cropped portion of the image
		/// </summary>
		/// <param name="Photo">The image to crop</param>
		/// <returns>The cropped photo</returns>
		public static Image SquareCropImage(this Image Photo)
		{
			int sourceWidth = Photo.Width;
			int sourceHeight = Photo.Height;
			int destSize = Photo.Height;
			int offset = (Photo.Width - destSize) / 2;

			if (sourceWidth < sourceHeight)
			{
				destSize = Photo.Width;
				offset = (Photo.Height - destSize) / 2;
			}

			Bitmap bmp = new Bitmap(destSize, destSize,
						 PixelFormat.Format24bppRgb);
			bmp.SetResolution(Photo.HorizontalResolution, Photo.VerticalResolution);

			Graphics g = Graphics.FromImage(bmp);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			if(sourceWidth >= sourceHeight)
			{
				g.DrawImage(Photo, new Point(-offset, 0));
			}
			else
			{
				g.DrawImage(Photo, new Point(0, -offset));
			}

			g.Dispose();
			return bmp;
		}

		/// <summary>
		/// Shuffles a list into a random order
		/// </summary>
		/// <typeparam name="T">Type of Item in the List to shuffle</typeparam>
		/// <param name="list">The list to shuffle</param>
		public static void Shuffle<T>(this IList<T> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}