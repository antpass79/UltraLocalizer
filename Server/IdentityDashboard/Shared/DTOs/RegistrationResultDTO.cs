using System.Collections.Generic;

namespace MyLabLocalizer.IdentityDashboard.Shared.DTOs
{
    public class RegistrationResultDTO
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
