﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Shane.Church.Web.Data.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Shane.Church.Web.v2015.Blog.Controllers
{
    [Route("api/[controller]")]
    public class BlogEntryController : Controller
    {
        // GET: api/blogentry/recent
        [HttpGet("recent/{count:int=10}")]
        public IEnumerable<Journal> Recent(int count)
        {
            DataContext context = new DataContext();
            var entries = context.Journals.OrderByDescending(it => it.EntryDate).Take(count).ToList();
            return entries;
        }

        // GET api/values/5
        [HttpGet("{id:int}")]
        public Journal Get(int id)
        {
            DataContext context = new DataContext();
            var entry = context.Journals.SingleOrDefault(it => it.Id == id);
            return entry;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
