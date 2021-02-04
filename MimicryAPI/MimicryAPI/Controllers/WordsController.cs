using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicryAPI.Helpers;
using MimicryAPI.Models;
using MimicryAPI.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MimicryAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _repository;
        public WordsController(IWordRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public ActionResult Get([FromQuery]WordUrlQuery wordUrlQuery)
        {
            var words = _repository.Get(wordUrlQuery);

            if (wordUrlQuery.PageNumber.Value > words.Pagination.TotalPages)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(words.Pagination));

            return Ok(words);
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Get(int id)
        {
            var word = _repository.Get(id);

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
            _repository.Add(word);

            return Created($"/api/words/{word.Id}", word);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _repository.Get(id);

            if (obj == null)
            {
                return NotFound();
            }

            word.Id = id;
            word.ModifiedDate = DateTime.Now;
            _repository.Update(word);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var word = _repository.Get(id);

            if (word == null)
            {
                return NotFound();
            }

            _repository.Delete(word);

            return NoContent();
        }

    }
}
