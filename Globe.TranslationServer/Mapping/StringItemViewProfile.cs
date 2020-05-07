using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;

namespace Globe.TranslationServer.Mapping
{
    public class StringItemViewProfile : Profile
    {
        public StringItemViewProfile()
        {
            CreateMap<GroupedStringEntity, StringItemViewDTO>()
                .ForMember(dest => dest.ComponentNamespace, opt => opt.MapFrom(src => src.ComponentNamespace))
                .ForMember(dest => dest.InternalNamespace, opt => opt.MapFrom(src => src.InternalNamespace))
                .ForMember(dest => dest.Concept, opt => opt.MapFrom(src => "To Find"))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => "To Find"))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "To Find"))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => "To Find"));
        }
    }
}
