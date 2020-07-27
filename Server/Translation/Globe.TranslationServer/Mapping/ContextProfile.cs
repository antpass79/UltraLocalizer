using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;

namespace Globe.TranslationServer.Mapping
{
    public class ContextProfile : Profile
    {
        public ContextProfile()
        {
            CreateMap<LocContexts, ContextDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContextName));
        }
    }
}
