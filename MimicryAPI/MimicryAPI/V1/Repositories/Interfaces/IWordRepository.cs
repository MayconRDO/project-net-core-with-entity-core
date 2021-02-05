using MimicryAPI.Helpers;
using MimicryAPI.V1.Models;

namespace MimicryAPI.V1.Repositories.Interfaces
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
