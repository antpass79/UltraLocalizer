using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.Mapping
{
    public class LoginResultProfile : Profile
    {
        public LoginResultProfile()
        {
            CreateMap<LoginResult, LoginResultDTO>();
            CreateMap<LoginResultDTO, LoginResult>();
        }
    }
}
