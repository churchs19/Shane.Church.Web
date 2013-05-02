using Shane.Church.Web.Data.Models;
using Shane.Church.Web.Common.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using System.Web;

namespace Shane.Church.Web.v2012.Controllers
{
	public class PhotoCategoryApiController : ApiController
	{
		// GET api/<controller>
		public IQueryable<PhotoCategoryDTO> Get()
		{
			DataContext context = new DataContext();
			List<PhotoCategoryDTO> categories = Mapper.Map<List<PhotoCategoryDTO>>(context.Pages.ToList());
			return categories.AsQueryable();
		}

		// GET api/<controller>/5
		public PhotoCategoryDTO Get(string id)
		{
			DataContext context = new DataContext();
			var page = context.Pages.Find(id);
			if (page == null)
				throw new HttpException(404, "Category not found.");
			else
				return Mapper.Map<PhotoCategoryDTO>(page);
		}

		// POST api/<controller>
		public void Post([FromBody]PhotoCategoryDTO value)
		{
			DataContext context = new DataContext();
			var page = context.Pages.Find(value.PageId);
			if (page == null)
			{
				page = Mapper.Map<PhotoCategoryDTO, Page>(value);
				context.Pages.Add(page);
			}
			else
			{
				page = Mapper.Map<PhotoCategoryDTO, Page>(value, page);
			}
			context.SaveChanges();
		}

		// PUT api/<controller>/5
		public void Put([FromBody]PhotoCategoryDTO value)
		{
			Post(value);
		}

		// DELETE api/<controller>/5
		public void Delete(string id)
		{
			DataContext context = new DataContext();
			var page = context.Pages.Find(id);
			if (page == null)
				throw new HttpException(404, "Category not found.");
			else
			{
				context.Pages.Remove(page);
				context.SaveChanges();
			}
		}
	}
}