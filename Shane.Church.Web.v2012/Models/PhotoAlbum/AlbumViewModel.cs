using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shane.Church.Web.v2012.Models.PhotoAlbum
{
	public class AlbumViewModel
	{
		public AlbumViewModel()
		{
			Photos = new List<PhotoViewModel>();
			PageSize = 4;
		}

		public string ID { get; set; }
		public string DisplayName { get; set; }
		public string Title { get; set; }
		public int PageNumber { get; set; }
		public int LastPageNumber 
		{
			get
			{
				int page = (TotalPhotos / PageSize) - 1;
				if (TotalPhotos % PageSize != 0)
				{
					page++;
				}
				return page;
			}
		}
		public int PageSize { get; set; }
		public int TotalPhotos { get; set; }
		public IEnumerable<PhotoViewModel> Photos { get; set; }
	}
}