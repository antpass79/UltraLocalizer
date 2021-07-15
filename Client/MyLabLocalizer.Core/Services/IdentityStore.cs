using MyLabLocalizer.Core.Extensions;
using MyLabLocalizer.Core.Identity;
using System.Collections.Generic;
using System.Security.Principal;

namespace MyLabLocalizer.Core.Services
{
    public class IdentityStore : IIdentityStore
    {
        public IPrincipal Principal { get; private set; } = new AnonymousPrincipal();

        public IIdentity Identity => Principal.Identity;

        public bool IsAuthenticated => Principal.Identity.IsAuthenticated;

        public IEnumerable<string> UserRoles => Principal.Identity.GetRoles();

        public void Store(IPrincipal principal)
        {
            Principal = principal ?? new AnonymousPrincipal();
        }
    }
}
