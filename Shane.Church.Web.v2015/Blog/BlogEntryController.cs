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
        [HttpGet("recent/{count:int=3}")]
        public IEnumerable<BlogEntry> Recent(int count, bool summary = false)
        {
            DataContext context = new DataContext();
            var journalEntries = context.Journals.OrderByDescending(it => it.EntryDate).Take(count).ToList();
			List<BlogEntry> entries = new List<BlogEntry>();
			entries.AddRange(journalEntries.Select(it => BlogEntry.fromJournal(it, summary)));
            return entries;
        }

        // GET api/values/5
        [HttpGet("{id:int}")]
        public BlogEntry Get(int id)
        {
            DataContext context = new DataContext();
            var entry = context.Journals.SingleOrDefault(it => it.Id == id);		
            return entry != null ? BlogEntry.fromJournal(entry) : new BlogEntry();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]BlogEntry value)
        {
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
