using MimicryAPI.Helpers;
using MimicryAPI.Models;
using System.Collections.Generic;

namespace MimicryAPI.Repositories.Interfaces
{
    public interface IWordRepository
    {
        PaginationList<Word> Get(WordUrlQuery query);
        Word Get(int id);
        void Add(Word word);
        void Update(Word word);
        void Delete(Word word);
    }
}
