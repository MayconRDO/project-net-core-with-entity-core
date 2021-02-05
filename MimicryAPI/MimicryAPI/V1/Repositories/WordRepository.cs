using Microsoft.EntityFrameworkCore;
using MimicryAPI.DataBase;
using MimicryAPI.Helpers;
using MimicryAPI.V1.Models;
using MimicryAPI.V1.Repositories.Interfaces;
using System;
using System.Linq;

namespace MimicryAPI.V1.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly MimicryContext _context;
        public WordRepository(MimicryContext context)
        {
            _context = context;
        }

        public PaginationList<Word> Get(WordUrlQuery query)
        {
            var paginationList = new PaginationList<Word>();
            var words = _context.Words.AsNoTracking().AsQueryable();

            if (query.Date.HasValue)
            {
                words = words.Where(w => w.CreationDate > query.Date.Value || w.ModifiedDate > query.Date.Value);
            }

            if (query.PageNumber.HasValue)
            {
                var totalRecords = words.Count();

                words = words.Skip((query.PageNumber.Value - 1) * query.RecordPerPage.Value).Take(query.RecordPerPage.Value);

                var pagination = new Pagination()
                {
                    NumberPage = query.PageNumber.Value,
                    RecordPerPage = query.RecordPerPage.Value,
                    TotalPages = totalRecords,
                    TotalRecord = (int)Math.Ceiling((double)totalRecords / query.RecordPerPage.Value)
                };

                paginationList.Pagination = pagination;
            }

            paginationList.Results.AddRange(words.ToList());

            return paginationList;
        }

        public Word Get(int id)
        {
            return _context.Words.AsNoTracking().FirstOrDefault(w => w.Id == id);
        }
        public void Add(Word word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();
        }
        public void Update(Word word)
        {
            _context.Words.Update(word);
            _context.SaveChanges();
        }
        public void Delete(Word word)
        {
            word.Active = false;
            _context.Words.Update(word);
            _context.SaveChanges();

        }
    }
}
