using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;

namespace Globe.TranslationServer.Mapping
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<DBLanguage, LanguageDTO>();
            CreateMap<LanguageDTO, DBLanguage>();
        }
    }
}
