using System.Collections.Generic;

namespace Globe.Identity.Models
{
    public class RegistrationResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
