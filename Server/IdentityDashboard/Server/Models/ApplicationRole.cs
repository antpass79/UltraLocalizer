using Microsoft.AspNetCore.Identity;

namespace MyLabLocalizer.IdentityDashboard.Server.Models
{
    public class ApplicationRole : IdentityRole
    {

        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }

        public string Description { get; set; }
    }
}
