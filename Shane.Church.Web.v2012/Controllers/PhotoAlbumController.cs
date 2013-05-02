using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shane.Church.Web.v2012.Models.PhotoAlbum;
using Shane.Church.Web.v2012.Models;
using Shane.Church.Web.v2012.Helpers;
using System.Drawing;
using System.IO;
using System.Web.UI;
using Shane.Church.Web.Data.Models;
using Shane.Church.Web.Data;
using Shane.Church.Web.Cloud;

namespace Shane.Church.Web.Controllers
{
	public class PhotoAlbumController : Controller
	{
		IBlobStorage _storageController;

		public PhotoAlbumController(IBlobStorage storage)
		{
			if (storage == null)
				throw new ArgumentNullException("storage");
			_storageController = storage;
		}

		public ActionResult Index()
		{
			IndexViewModel viewModel = new IndexViewModel();

			DataContext model = new DataContext();
			var pages = (from pg in model.Pages
						 join p in model.Photos on pg.PageId equals p.PageId into pageGroup
						 select new PageItem()
						 {
							 ID = pg.PageId,
							 DisplayName = pg.DisplayName,
							 Title = pg.PageTitle,
							 Latitude = pg.Latitude,
							 Longitude = pg.Longitude,
							 SortOrder = pg.SortOrder,
							 UpdatedDate = (from p2 in pageGroup
											select p2.UpdatedDate).Max(),
							 Count = pageGroup.Count()
						 }).OrderBy(m => m.UpdatedDate);


			viewModel.Pages = pages;

			return View(viewModel);
		}

		public ActionResult Album(string id, int pageNumber = 0, string outputType = "html")
		{
			AlbumViewModel viewModel = new AlbumViewModel();

			DataContext model = new DataContext();

			var selectedPage = (from pg in model.Pages
						where pg.PageId == id && pg.Active.HasValue && pg.Active.Value == -1
						select pg);
			if (selectedPage.Count() > 0)
			{
				Shane.Church.Web.Data.Models.Page p = selectedPage.First();
				viewModel.ID = p.PageId;
				viewModel.DisplayName = p.DisplayName;
				viewModel.Title = p.PageTitle;
			}

			viewModel.Photos = (from p in model.Photos
								  join pg in model.Pages on p.PageId equals pg.PageId
								  where p.PageId == id && p.Active.HasValue && p.Active.Value == -1
								  orderby p.UpdatedDate descending
								  select new PhotoViewModel() { ID = p.PhotoId, CategoryID = pg.PageId, CategoryName = pg.DisplayName, Caption = p.Caption, File = p.File, UpdatedDate = p.UpdatedDate })
								  .Skip(pageNumber * viewModel.PageSize).Take(viewModel.PageSize);

			viewModel.TotalPhotos = (from p in model.Photos
									 where p.PageId == id && p.Active.HasValue && p.Active.Value == -1
									 select p).Count();

			viewModel.PageNumber = pageNumber;

			if (outputType.ToLower() == "json")
			{
				JsonResult result = new JsonResult();
				result.Data = viewModel;
				result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
				return result;
			}
			else
			{
				return View(viewModel);
			}
		}

		public ActionResult View(long id)
		{
			DataContext model = new DataContext();
			var photo = (from p in model.Photos
						 join pg in model.Pages on p.PageId equals pg.PageId
						 where p.PhotoId == id
						 select new PhotoViewModel() { ID = p.PhotoId, CategoryID = pg.PageId, CategoryName = pg.DisplayName, Caption = p.Caption, File = p.File, UpdatedDate = p.UpdatedDate });

			return View(photo.FirstOrDefault());
		}

		[OutputCache(CacheProfile="PhotoProfile")]
		public ActionResult Image(long id)
		{
			DataContext model = new DataContext();
			var photo = (from p in model.Photos
						 join pg in model.Pages on p.PageId equals pg.PageId
						 where p.PhotoId == id
						 select new PhotoViewModel() { ID = p.PhotoId, CategoryID = pg.PageId, CategoryName = pg.DisplayName, Caption = p.Caption, File = p.File, UpdatedDate = p.UpdatedDate }).FirstOrDefault();

			try
			{
				Stream data = _storageController.ReadObject(photo.File);

				return File(data, "image/jpeg");
			}
			catch
			{
				return new HttpStatusCodeResult(404);
			}
		}

		[OutputCache(CacheProfile = "PhotoProfile")]
		public ActionResult ResizedImage(long id, int minSize = 0, int maxSize = 200, bool square = false)
		{
			DataContext model = new DataContext();
			var photo = (from p in model.Photos
						 join pg in model.Pages on p.PageId equals pg.PageId
						 where p.PhotoId == id
						 select new PhotoViewModel() { ID = p.PhotoId, CategoryID = pg.PageId, CategoryName = pg.DisplayName, Caption = p.Caption, File = p.File, UpdatedDate = p.UpdatedDate }).FirstOrDefault();

			try
			{
				Bitmap bmp = _storageController.ReadImage(photo.File);

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

				MemoryStream mStream = new MemoryStream();
				if (!square)
				{
					bmp.ScaleByPercent(percent).Save(mStream, System.Drawing.Imaging.ImageFormat.Jpeg);
				}
				else
				{
					bmp.ScaleByPercent(percent).SquareCropImage().Save(mStream, System.Drawing.Imaging.ImageFormat.Jpeg);
				}
				mStream.Seek(0, SeekOrigin.Begin);

				return File(mStream, "image/jpeg");
			}
			catch/*(Exception ex)*/
			{
				return new HttpStatusCodeResult(404);
			}
		}

		[OutputCache(CacheProfile = "PhotoProfile")]
		public ActionResult CategoryImage(string page, float minSize = 200f, bool isSquare = false)
		{
			string CategoryId = page;
			float maxSize = 200f;

			Bitmap bmp = null;
			try
			{
				if (CategoryId != null)
				{
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

					MemoryStream ms = new MemoryStream();
					if (!isSquare)
					{
						bmp.ScaleByPercent(percent).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					}
					else
					{
						bmp.ScaleByPercent(percent).SquareCropImage().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					}

					ms.Seek(0, SeekOrigin.Begin);
					return File(ms, "image/png");
				}
				else
				{
					return new HttpStatusCodeResult(404);
				}
			}
			catch
			{
				return new HttpStatusCodeResult(500);
			}
		}
	}
}
