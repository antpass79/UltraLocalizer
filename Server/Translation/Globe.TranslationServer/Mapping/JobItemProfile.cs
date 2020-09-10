using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;

namespace Globe.TranslationServer.Mapping
{
    public class JobItemProfile : Profile
    {
        public JobItemProfile()
        {
            CreateMap<JobList, JobItemDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IDJob))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.JobName))
                .ForMember(dest => dest.IsoId, opt => opt.MapFrom(src => src.IDIso));
            CreateMap<JobItemDTO, JobList>()
                .ForMember(dest => dest.IDJob, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IDIso, opt => opt.MapFrom(src => src.IsoId));
        }
    }
}
