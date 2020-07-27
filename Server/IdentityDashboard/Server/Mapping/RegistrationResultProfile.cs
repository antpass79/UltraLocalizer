using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.Mapping
{
    public class RegistrationResultProfile : Profile
    {
        public RegistrationResultProfile()
        {
            CreateMap<RegistrationResult, RegistrationResultDTO>();
            CreateMap<RegistrationResultDTO, RegistrationResult>();
        }
    }
}
