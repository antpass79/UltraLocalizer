using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.Mapping
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
