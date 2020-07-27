using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;

namespace Globe.TranslationServer.Mapping
{
    public class ConceptViewProfile : Profile
    {
        public ConceptViewProfile()
        {
            CreateMap<GroupedStringEntity, ConceptViewDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ConceptID))
                .ForMember(dest => dest.ComponentNamespace, opt => opt.MapFrom(src => src.ComponentNamespace))
                .ForMember(dest => dest.InternalNamespace, opt => opt.MapFrom(src => src.InternalNamespace))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LocalizationID))
                .ForMember(dest => dest.ContextViews, opt => opt.MapFrom(src => src.Group));
        }
    }
}
