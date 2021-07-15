using System.Collections.Generic;
using System.Security.Principal;

namespace MyLabLocalizer.Core.Services
{
    public interface IIdentityStore
    {
        IPrincipal Principal { get; }
        IIdentity Identity { get; }
        bool IsAuthenticated { get; }
        IEnumerable<string> UserRoles { get; }

        void Store(IPrincipal Principal);
    }
}
