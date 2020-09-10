using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;

namespace Globe.TranslationServer.Mapping
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<DBLanguage, LanguageDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IDLanguage))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DataString))
                .ForMember(dest => dest.IsoCoding, opt => opt.MapFrom(src => "DBLanguage hasn't IsoCoding"));
            CreateMap<LanguageDTO, DBLanguage>()
                .ForMember(dest => dest.IDLanguage, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DataString, opt => opt.MapFrom(src => src.Name));
        }
    }
}
