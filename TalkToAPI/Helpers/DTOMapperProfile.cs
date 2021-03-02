using AutoMapper;
using System.Collections.Generic;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;

namespace TalkToAPI.Helpers
{
    /// <summary>
    /// Classe de configuração AutoMapper
    /// </summary>
    public class DTOMapperProfile : Profile
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public DTOMapperProfile()
        {
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Name, orig => orig.MapFrom(src => src.FullName));

            CreateMap<Message, MessageDTO>();
        }
    }
}
