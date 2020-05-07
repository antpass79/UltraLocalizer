using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Mapping
{
    public class ContextViewItemProfile : Profile
    {
        public ContextViewItemProfile()
        {
            CreateMap<StringEntity, ContextViewItemDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContextName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.StringType))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.DataString));
        }
    }
}
