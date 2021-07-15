using AutoMapper;
using MyLabLocalizer.IdentityDashboard.Server.Models;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;

namespace MyLabLocalizer.IdentityDashboard.Server.Mapping
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
            CreateMap<ApplicationUserDTO, ApplicationUser>();
        }
    }
}
