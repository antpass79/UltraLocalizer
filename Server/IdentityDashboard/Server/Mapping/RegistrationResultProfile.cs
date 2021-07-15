using AutoMapper;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace MyLabLocalizer.IdentityDashboard.Server.Mapping
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
