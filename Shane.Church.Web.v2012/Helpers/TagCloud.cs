using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shane.Church.Web.v2012.Models.Blog;
using Shane.Church.Web.v2012.Helpers;
using Shane.Church.Web.Data.Models;

namespace Shane.Church.Web.v2012.Helpers
{
	public static class TagCloud
	{
		public static IEnumerable<TagCloudItem> GetBlogTagCloud(this DataContext model, int numBuckets, int numTags = 20)
		{
			List<TagCloudItem> items = new List<TagCloudItem>();

			Dictionary<string, int> cloud = new Dictionary<string, int>();
			var cloudGroups = (from t in model.JournalTags
							   group t by t.Tag into g
							   select g);
			foreach (IGrouping<string, JournalTags> group in cloudGroups.Take(numTags))
			{
				cloud.Add(group.Key, group.Count());
			}

			var cloudTrimmed = cloud.OrderByDescending(m => m.Value).Take(numTags);

			//Determine cutoff points
			int maxVal = cloudTrimmed.Max(m => m.Value);
			int minVal = cloudTrimmed.Min(m => m.Value);

			int step = (maxVal - minVal) / (numBuckets - 1);
			if (step == 0)
				step = 1;

			var ceilings = new int[numBuckets - 1];
			for (int i = numBuckets - 2; i >= 0; i--)
			{
				ceilings[i] = maxVal - (i * step);
			}

			var groupings = cloudTrimmed.GroupBy(item => ceilings.Last(ceiling => ceiling >= item.Value));

			int weight = groupings.Count() + 1;
			foreach (IGrouping<int, KeyValuePair<string, int>> group in groupings)
			{
				weight--;
				for (int i = 0; i < group.Count(); i++ )
				{
					TagCloudItem cloudItem = new TagCloudItem() { Tag = group.ElementAt(i).Key, Weight = weight };
					items.Add(cloudItem);
				}
			}

			items.Shuffle();

			return items;
		}
	}
}