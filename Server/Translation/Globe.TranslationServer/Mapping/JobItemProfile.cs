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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IDJob));
            CreateMap<JobItemDTO, JobList>();
        }
    }
}
