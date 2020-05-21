using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Globe.TranslationServer.Mapping
{
    public class StringViewItemProfile : Profile
    {
        public StringViewItemProfile()
        {
            CreateMap<GroupedStringEntity, ConceptViewDTO>()
                .ForMember(dest => dest.ComponentNamespace, opt => opt.MapFrom(src => src.ComponentNamespace))
                .ForMember(dest => dest.InternalNamespace, opt => opt.MapFrom(src => src.InternalNamespace))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LocalizationID))
                .ForMember(dest => dest.ContextViews, opt => opt.MapFrom(src => src.Group));
        }
    }
}
