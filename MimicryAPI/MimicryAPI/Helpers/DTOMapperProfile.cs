using AutoMapper;
using MimicryAPI.V1.Models;
using MimicryAPI.V1.Models.DTO;
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
