using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.Mapping
{
    public class RegistrationProfile : Profile
    {
        public RegistrationProfile()
        {
            CreateMap<Registration, RegistrationDTO>();
            CreateMap<RegistrationDTO, Registration>();
        }
    }
}
