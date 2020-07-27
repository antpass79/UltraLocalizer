using Globe.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Globe.Identity.AdministrativeDashboard.Server.Models
{
    public class ApplicationUser : GlobeUser
    {
        public ApplicationUser()
        {
            CreateDate = DateTime.Now;
            IsApproved = false;
            LastLoginDate = DateTime.Now;
            LastActivityDate = DateTime.Now;
            LastPasswordChangedDate = DateTime.Now;
            LastLockoutDate = DateTime.Parse("1/1/1754");
            FailedPasswordAnswerAttemptWindowStart = DateTime.Parse("1/1/1754");
            FailedPasswordAttemptWindowStart = DateTime.Parse("1/1/1754");
        }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new List<IdentityUserRole<string>>();

        // For IdentityAdministrativeDashboard
        public int ApplicationId { get; set; }
        // For IdentityAdministrativeDashboard1
        //public Guid ApplicationId { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string MobilePIN { get; set; }
        public string LoweredEmail { get; set; }
        public string LoweredUserName { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime? LastLockoutDate { get; set; }
        // For IdentityAdministrativeDashboard
        public int? FailedPasswordAttemptCount { get; set; }
        // For IdentityAdministrativeDashboard1
        //public int FailedPasswordAttemptCount { get; set; }
        public DateTime? FailedPasswordAttemptWindowStart { get; set; }
        // For IdentityAdministrativeDashboard
        public int? FailedPasswordAnswerAttemptCount { get; set; }
        // For IdentityAdministrativeDashboard1
        //public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }
        public string Comment { get; set; }
    }
}
