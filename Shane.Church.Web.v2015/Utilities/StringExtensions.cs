using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Shane.Church.Web.Utilities
{
    public static class StringExtensions
    {
		public static string TruncateHtml(this string input, int length = 300,
										   string ommission = "...")
		{
			if (input == null || input.Length < length)
				return input;
			int iNextSpace = input.LastIndexOf(" ", length);
			return string.Format("{0}" + ommission, input.Substring(0, (iNextSpace > 0) ?
																  iNextSpace : length).Trim());
		}

		public static string StripTags(this string markup)
		{
			try
			{
				char[] array = new char[markup.Length];
				int arrayIndex = 0;
				bool inside = false;

				for (int i = 0; i < markup.Length; i++)
				{
					char let = markup[i];
					if (let == '<')
					{
						inside = true;
						continue;
					}
					if (let == '>')
					{
						inside = false;
						continue;
					}
					if (!inside)
					{
						array[arrayIndex] = let;
						arrayIndex++;
					}
				}
				return new string(array, 0, arrayIndex);
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}