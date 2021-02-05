using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicryAPI.Helpers;
using MimicryAPI.Models;
using MimicryAPI.Models.DTO;
using MimicryAPI.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MimicryAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _repository;
        private readonly IMapper _mapper;

        public WordsController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetAll")]
        public ActionResult Get([FromQuery]WordUrlQuery query)
        {
            var words = _repository.Get(query);

            if (words.Results.Count == 0)
            {
                NotFound();
            }

            PaginationList<WordDTO> wordsDto = CreateLinksWord(query, words);

            return Ok(wordsDto);
        }

        private PaginationList<WordDTO> CreateLinksWord(WordUrlQuery query, PaginationList<Word> words)
        {
            var wordsDto = _mapper.Map<PaginationList<Word>, PaginationList<WordDTO>>(words);

            foreach (var word in wordsDto.Results)
            {
                word.Links = new List<LinkDTO>();
                word.Links.Add(new LinkDTO("self", Url.Link("Get", new { id = word.Id }), "GET"));
            }

            wordsDto.Links.Add(new LinkDTO("self", Url.Link("GetAll", query), "GET"));

            if (words.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(words.Pagination));

                if (query.PageNumber + 1 <= words.Pagination.TotalPages)
                {
                    var queryString = new WordUrlQuery()
                    {
                        PageNumber = query.PageNumber + 1,
                        RecordPerPage = query.RecordPerPage,
                        Date = query.Date
                    };
                    wordsDto.Links.Add(new LinkDTO("next", Url.Link("GetAll", queryString), "GET"));
                }

                if (query.PageNumber - 1 > 0)
                {
                    var queryString = new WordUrlQuery()
                    {
                        PageNumber = query.PageNumber - 1,
                        RecordPerPage = query.RecordPerPage,
                        Date = query.Date
                    };
                    wordsDto.Links.Add(new LinkDTO("prev", Url.Link("GetAll", queryString), "GET"));
                }
            }

            return wordsDto;
        }

        [HttpGet("{id}", Name = "Get")]
        public ActionResult Get(int id)
        {
            var word = _repository.Get(id);

            if (word == null)
            {
                return NotFound();
            }

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links = new List<LinkDTO>();
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Get", new { id = wordDTO.Id }), "GET"));
            wordDTO.Links.Add(new LinkDTO("update", Url.Link("Update", new { id = wordDTO.Id }), "PUT"));
            wordDTO.Links.Add(new LinkDTO("delete", Url.Link("Delete", new { id = wordDTO.Id }), "DELETE"));

            return Ok(wordDTO);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]Word word)
        {
            word.CreationDate = DateTime.Now;
            _repository.Add(word);

            return Created($"/api/words/{word.Id}", word);
        }

        [HttpPut("{id}", Name = "Update")]
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

        [HttpDelete("{id}", Name = "Delete")]
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
