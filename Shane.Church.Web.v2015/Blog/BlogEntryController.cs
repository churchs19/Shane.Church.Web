using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Shane.Church.Web.Data.Models;
using Shane.Church.Web.Blog;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Shane.Church.Web.v2015.Blog.Controllers
{
    [Route("api/[controller]")]
    public class BlogEntryController : Controller
    {
        // GET: api/blogentry/recent
        [HttpGet]
        public IEnumerable<BlogEntry> GetList(int count = 3, bool summary = true)
        {
            DataContext context = new DataContext();
            var journalEntries = context.Journals.OrderByDescending(it => it.EntryDate).Take(count).ToList();
			List<BlogEntry> entries = new List<BlogEntry>();
			entries.AddRange(journalEntries.Select(it => BlogEntry.fromJournal(it, summary)));
            return entries;
        }

        // GET api/values/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            DataContext context = new DataContext();
            var entry = context.Journals.SingleOrDefault(it => it.Id == id);
			if (entry != null)
			{
				return new ObjectResult(BlogEntry.fromJournal(entry));
			}
			else
			{
				return HttpNotFound();
			}
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]BlogEntry value)
        {
			if (!ModelState.IsValid)
			{
				Context.Response.StatusCode = 400;
			}
			else
			{
				//item.Id = 1 + _items.Max(x => (int?)x.Id) ?? 0;
				//_items.Add(item);

				//string url = Url.RouteUrl("GetByIdRoute", new { id = item.Id },
				//	Request.Scheme, Request.Host.ToUriComponent());

				//Context.Response.StatusCode = 201;
				//Context.Response.Headers["Location"] = url;
			}
		}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]BlogEntry value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
