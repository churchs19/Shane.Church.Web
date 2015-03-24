using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Shane.Church.Web.Photo.Instagram.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Shane.Church.Web.Photo.Controllers
{
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
		private async Task<MediaResponse> InstagramRecent()
		{
			//https://api.instagram.com/v1/users/1511680150/media/recent?client_id=b6f5ef5726a74224b8dbc213f1f64432
			HttpClient client = new HttpClient();
			var photoStream = await client.GetAsync("https://api.instagram.com/v1/users/1511680150/media/recent?client_id=b6f5ef5726a74224b8dbc213f1f64432");
			photoStream.EnsureSuccessStatusCode();
			var mediaResponse = await photoStream.Content.ReadAsAsync<MediaResponse>();
			return mediaResponse;			
		}


        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
			try
			{
				var response = await InstagramRecent();
				var items = response.data.OrderByDescending(it => it.created_time)
					.Take(8)
					.Select(it => new PhotoItem()
					{
						Id = it.id,
						PhotoType = PhotoType.Instagram,
						Caption = it.caption.text,
						SmallImage = it.images.low_resolution.url,
						LargeImage = it.images.standard_resolution.url
					}).ToList();
				return new ObjectResult(items);
			}
			catch
			{
				return new HttpStatusCodeResult(500);
			}
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
