using Microsoft.AspNetCore.Mvc;
using MimicryAPI.DataBase;
using MimicryAPI.Models;

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
        public ActionResult Get()
        {
            return Ok(_context.Words);
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Get(int id)
        {
            return Ok(_context.Words.Find(id));
        }

        [Route("")]
        [HttpPost]
        public ActionResult Add(Word word)
        {
            _context.Words.Add(word);

            return Ok();
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, Word word)
        {
            _context.Words.Update(word);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _context.Words.Remove(_context.Words.Find(id));

            return Ok();
        }

    }
}
