using AutoMapper;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace MyLabLocalizer.IdentityDashboard.Server.Mapping
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
