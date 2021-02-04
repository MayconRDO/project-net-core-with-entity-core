using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicryAPI.DataBase;
using MimicryAPI.Helpers;
using MimicryAPI.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MimicryAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly MimicryContext _context;
        public WordsController(MimicryContext context)
        {
            _context = context;
        }

        [Route("")]
        [HttpGet]
        public ActionResult Get([FromQuery]WordUrlQuery wordUrlQuery)
        {
            var words = _context.Words.AsQueryable();

            if (wordUrlQuery.Date.HasValue)
            {
                words = words.Where(w => w.CreationDate > wordUrlQuery.Date.Value /*|| w.ModifiedDate > date.Value*/);
            }

            if (wordUrlQuery.PageNumber.HasValue)
            {
                var totalRecords = words.Count();

                words = words.Skip((wordUrlQuery.PageNumber.Value - 1) * wordUrlQuery.RecordPerPage.Value).Take(wordUrlQuery.RecordPerPage.Value);

                var pagination = new Pagination()
                {
                    NumberPage = wordUrlQuery.PageNumber.Value,
                    RecordPerPage = wordUrlQuery.RecordPerPage.Value,
                    TotalPages = totalRecords,
                    TotalRecord = (int)Math.Ceiling((double)totalRecords / wordUrlQuery.RecordPerPage.Value)
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

                if (wordUrlQuery.PageNumber.Value > pagination.TotalPages)
                {
                    return NotFound();
                }
            }

            return Ok(words);
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Get(int id)
        {
            var word = _context.Words.Find(id);

            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]Word word)
        {
            word.CreationDate = DateTime.Now;
            _context.Words.Add(word);
            _context.SaveChanges();

            return Created($"/api/words/{word.Id}", word);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _context.Words.AsNoTracking().FirstOrDefault(w => w.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            word.Id = id;
            word.ModifiedDate = DateTime.Now;
            _context.Words.Update(word);
            _context.SaveChanges();

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var word = _context.Words.Find(id);

            if (word == null)
            {
                return NotFound();
            }

            word.Active = false;
            _context.Words.Update(word);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
