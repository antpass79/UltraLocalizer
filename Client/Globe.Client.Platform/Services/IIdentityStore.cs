using System.Collections.Generic;
using System.Security.Principal;

namespace Globe.Client.Platform.Services
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
