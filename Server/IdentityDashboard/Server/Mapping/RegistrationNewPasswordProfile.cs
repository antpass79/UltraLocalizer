using AutoMapper;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace MyLabLocalizer.IdentityDashboard.Server.Mapping
{
    public class RegistrationNewPasswordProfile : Profile
    {
        public RegistrationNewPasswordProfile()
        {
            CreateMap<RegistrationNewPassword, RegistrationNewPasswordDTO>();
            CreateMap<RegistrationNewPasswordDTO, RegistrationNewPassword>();
        }
    }
}
