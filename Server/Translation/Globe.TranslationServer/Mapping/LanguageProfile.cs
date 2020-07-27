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
                .ForMember(dest => dest.ISOCoding, opt => opt.MapFrom(src => src.DataString));
            CreateMap<LanguageDTO, DBLanguage>()
                .ForMember(dest => dest.DataString, opt => opt.MapFrom(src => src.ISOCoding));
        }
    }
}
