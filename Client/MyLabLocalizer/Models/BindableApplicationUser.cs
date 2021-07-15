using MyLabLocalizer.Shared.DTOs;

namespace MyLabLocalizer.Models
{
    public class BindableApplicationUser : ApplicationUser
    {
        public string FullName => $"{LastName} {FirstName}";
        public string DisplayName => $"{FullName} ({UserName})";
    }
}
