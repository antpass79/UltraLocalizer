﻿using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;

namespace Globe.TranslationServer.Mapping
{
    public class ContextViewProfile : Profile
    {
        public ContextViewProfile()
        {
            CreateMap<StringEntity, ContextViewDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContextName))
                .ForMember(dest => dest.StringType, opt => opt.MapFrom(src => src.StringType))
                .ForMember(dest => dest.StringValue, opt => opt.MapFrom(src => src.DataString))
                .ForMember(dest => dest.StringId, opt => opt.MapFrom(src => src.IDString))
                .ForMember(dest => dest.OldStringId, opt => opt.MapFrom(src => src.OldIDString))
                .ForMember(dest => dest.Concept2ContextId, opt => opt.MapFrom(src => src.IDConcept2Context));
        }
    }
}