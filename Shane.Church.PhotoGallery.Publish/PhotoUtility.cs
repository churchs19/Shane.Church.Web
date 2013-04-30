using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Shane.Church.PhotoGallery.Publish
{
    public static class PhotoUtility
    {

        public static DateTime GetDateTaken(Image img)
        {
            int id = int.Parse("9003", System.Globalization.NumberStyles.AllowHexSpecifier);

            PropertyItem prop = img.GetPropertyItem(id);

            byte[] asciiBytes = prop.Value; 
            string strDate = System.Text.Encoding.ASCII.GetString(asciiBytes);

            DateTime dateCreated = new DateTime();
            // Pull out the year, month, day and time individually from the string
            string dateYear = strDate.Substring(0, 4);
            string dateMonth = strDate.Substring(5, 2);
            string dateDay = strDate.Substring(8, 2);
            string dateHourMinSec = strDate.Substring(11, 8);
            // Create a stringbuilder that is formatted for conversion to DateTime
            StringBuilder sbDateCreated = new StringBuilder();
            sbDateCreated.Append(dateMonth + "/");
            sbDateCreated.Append(dateDay + "/");
            sbDateCreated.Append(dateYear + " ");
            sbDateCreated.Append(dateHourMinSec);
            // Convert the string to DateTime
            dateCreated = Convert.ToDateTime(sbDateCreated.ToString());

            return dateCreated;
        }

        public static Image Resize(Image img, int maxSideSize)
        {
            int sourceWidth = img.Width;
            int sourceHeight = img.Height;

            float nPercent = 0;

            if (img.Width > img.Height)
            {
                nPercent = (float)maxSideSize / (float)img.Width;
            }
            else
            {
                nPercent = (float)maxSideSize / (float)img.Height;
            }

            int destWidth = (int)((float)sourceWidth * nPercent);
            int destHeight = (int)((float)sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static void SaveImageToTemp(string filename, Image img)
        {
            img.Save(Path.GetTempPath() + filename, ImageFormat.Jpeg);
        }

        public static void GetDisplaySizes(Image img, int maxWidth, int maxHeight, out int width, out int height)
        {
            width = int.MinValue;
            height = int.MinValue;
            if (img.Width > img.Height)
            {
                //landscape
                width = maxWidth;
                height = (int)((double)maxWidth / ((double)img.Width / (double)img.Height));

                if (height > maxHeight)
                {
                    //Height is limiting factor before width
                    height = maxHeight;
                    width = (int)(((double)img.Width / (double)img.Height) * (double)maxHeight);
                }
            }
            else
            {
                //portrait
                height = maxHeight;
                width = (int)(((double)img.Width / (double)img.Height) * (double)maxHeight);

                if (width > maxWidth)
                {
                    //Width is limiting factor before height
                    width = maxWidth;
                    height = (int)((double)maxWidth / ((double)img.Width / (double)img.Height));
                }
            }
        }
    }
}
