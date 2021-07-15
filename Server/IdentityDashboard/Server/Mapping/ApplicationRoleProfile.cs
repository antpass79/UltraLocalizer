using AutoMapper;
using MyLabLocalizer.IdentityDashboard.Server.Models;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;

namespace MyLabLocalizer.IdentityDashboard.Server.Mapping
{
    public class ApplicationRoleProfile : Profile
    {
        public ApplicationRoleProfile()
        {
            CreateMap<ApplicationRole, ApplicationRoleDTO>();
            CreateMap<ApplicationRoleDTO, ApplicationRole>();
        }
    }
}
