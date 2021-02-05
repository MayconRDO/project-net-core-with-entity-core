using AutoMapper;
using MimicryAPI.Models;
using MimicryAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicryAPI.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<Word, WordDTO>();
            CreateMap<PaginationList<Word>, PaginationList<WordDTO>>();
        }
    }
}
