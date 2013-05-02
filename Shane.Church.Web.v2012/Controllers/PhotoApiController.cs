using AutoMapper;
using Shane.Church.Web.Common.DataTransfer;
using Shane.Church.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Shane.Church.Web.v2012.Controllers
{
	public class PhotoApiController : ApiController
	{
		// GET api/<controller>
		public IQueryable<PhotoDTO> Get()
		{
			DataContext context = new DataContext();
			List<PhotoDTO> photos = Mapper.Map<List<PhotoDTO>>(context.Photos.ToList());
			return photos.AsQueryable();
		}

		// GET api/<controller>/5
		public PhotoDTO Get(long id)
		{
			DataContext context = new DataContext();
			var photo = context.Photos.Find(id);
			if (photo == null)
				throw new HttpException(404, "Photo not found.");
			else
				return Mapper.Map<PhotoDTO>(photo);
		}

		// POST api/<controller>
		public long Post([FromBody]PhotoDTO value)
		{
			DataContext context = new DataContext();
			var photo = context.Photos.Find(value.PhotoId);
			if (photo == null)
			{
				photo = Mapper.Map<PhotoDTO, Photo>(value);
				context.Photos.Add(photo);
			}
			else
			{
				photo = Mapper.Map<PhotoDTO, Photo>(value, photo);
			}
			context.SaveChanges();
			return photo.PhotoId;
		}

		// PUT api/<controller>/5
		public long Put([FromBody]PhotoDTO value)
		{
			return Post(value);
		}

		// DELETE api/<controller>/5
		public void Delete(long id)
		{
			DataContext context = new DataContext();
			var photo = context.Photos.Find(id);
			if (photo == null)
				throw new HttpException(404, "Photo not found.");
			else
			{
				context.Photos.Remove(photo);
				context.SaveChanges();
			}
		}
	}
}