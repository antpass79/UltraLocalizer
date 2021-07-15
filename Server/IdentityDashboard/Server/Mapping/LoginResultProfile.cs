using AutoMapper;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Globe.Identity.Models;

namespace MyLabLocalizer.IdentityDashboard.Server.Mapping
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
