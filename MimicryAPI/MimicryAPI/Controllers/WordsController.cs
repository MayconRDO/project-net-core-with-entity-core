using Microsoft.AspNetCore.Mvc;
using MimicryAPI.DataBase;
using MimicryAPI.Models;

namespace MimicryAPI.Controllers
{
    public class WordsController : ControllerBase
    {
        private readonly MimicryContext _context;
        public WordsController(MimicryContext context)
        {
            _context = context;
        }

        public ActionResult Get()
        {
            return Ok(_context.Words);
        }

        public ActionResult Get(int id)
        {
            return Ok(_context.Words.Find(id));
        }

        public ActionResult Add(Word word)
        {
            _context.Words.Add(word);

            return Ok();
        }

        public ActionResult Update(int id, Word word)
        {
            _context.Words.Update(word);

            return Ok();
        }

        public ActionResult Delete(int id)
        {
            _context.Words.Remove(_context.Words.Find(id));

            return Ok();
        }

    }
}
