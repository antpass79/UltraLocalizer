using Globe.Shared.DTOs;

namespace Globe.Client.Localizer.Models
{
    public class BindableApplicationUser : ApplicationUser
    {
        public string FullName => $"{LastName} {FirstName}";
        public string DisplayName => $"{FullName} ({UserName})";
    }
}
