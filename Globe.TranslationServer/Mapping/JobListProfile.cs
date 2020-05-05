using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;

namespace Globe.TranslationServer.Mapping
{
    public class JobListProfile : Profile
    {
        public JobListProfile()
        {
            CreateMap<JobList, JobListDTO>();
            CreateMap<JobListDTO, JobList>();
        }
    }
}
