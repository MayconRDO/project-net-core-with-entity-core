using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MimicryAPI.Helpers;
using MimicryAPI.V1.Models;
using MimicryAPI.V1.Models.DTO;
using MimicryAPI.V1.Repositories.Interfaces;
using Newtonsoft.Json;
using System;

namespace MimicryAPI.V1.Controllers
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _repository;
        private readonly IMapper _mapper;

        public WordsController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
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
                //word.Links = new List<LinkDTO>();
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

        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpGet("{id}", Name = "Get")]
        public ActionResult Get(int id)
        {
            var word = _repository.Get(id);

            if (word == null)
            {
                return NotFound();
            }

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            //wordDTO.Links = new List<LinkDTO>();
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Get", new { id = wordDTO.Id }), "GET"));
            wordDTO.Links.Add(new LinkDTO("update", Url.Link("Update", new { id = wordDTO.Id }), "PUT"));
            wordDTO.Links.Add(new LinkDTO("delete", Url.Link("Delete", new { id = wordDTO.Id }), "DELETE"));

            return Ok(wordDTO);
        }

        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            word.CreationDate = DateTime.Now;
            _repository.Add(word);

            var wordDTO = _mapper.Map<Word, WordDTO>(word);

            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Get", new { id = wordDTO.Id }), "GET"));

            return Created($"/api/words/{wordDTO.Id}", wordDTO);
        }

        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpPut("{id}", Name = "Update")]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _repository.Get(id);

            if (obj == null)
            {
                return NotFound();
            }

            if (word == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            word.Id = id;
            word.ModifiedDate = DateTime.Now;
            word.CreationDate = obj.CreationDate;

            _repository.Update(word);

            var wordDTO = _mapper.Map<Word, WordDTO>(word);

            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Get", new { id = wordDTO.Id }), "GET"));

            return Ok();
        }

        [MapToApiVersion("1.1")]
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
