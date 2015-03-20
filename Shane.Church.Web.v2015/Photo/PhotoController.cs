using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Shane.Church.Web.Photo.Controllers
{
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
		[HttpGet("instagram")]
		public async Task<string> InstagramRecent()
		{
			//https://api.instagram.com/v1/users/1511680150/media/recent?client_id=b6f5ef5726a74224b8dbc213f1f64432
			HttpClient client = new HttpClient();
			var photoStream = await client.GetAsync("https://api.instagram.com/v1/users/1511680150/media/recent?client_id=b6f5ef5726a74224b8dbc213f1f64432");

            return "";
		}


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
