using System.Collections.Generic;

namespace Globe.Identity.AdministrativeDashboard.Shared.DTOs
{
    public class RegistrationResultDTO
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
