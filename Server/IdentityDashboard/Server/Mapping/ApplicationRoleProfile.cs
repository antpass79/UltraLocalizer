using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;

namespace Globe.Identity.AdministrativeDashboard.Server.Mapping
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
