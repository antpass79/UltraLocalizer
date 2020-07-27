using System;
using System.Collections.Generic;
using System.Text;

namespace Globe.Identity.AdministrativeDashboard.Shared.DTOs
{
    public class UserWithRoles
    {
        public UserWithRoles()
        {
            this.User = new ApplicationUserDTO();
            this.Roles = new List<ApplicationRoleDTO>();
        }

        public ApplicationUserDTO User { get; set; }
        public IEnumerable<ApplicationRoleDTO> Roles { get; set; }
    }
}
