using AutoMapper;
using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Entities;

namespace MyLabLocalizer.LocalizationService.Mapping
{
    public class ContextProfile : Profile
    {
        public ContextProfile()
        {
            CreateMap<LocContext, ContextDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContextName));
        }
    }
}
