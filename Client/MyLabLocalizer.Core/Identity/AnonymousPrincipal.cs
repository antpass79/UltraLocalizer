using System.Security.Claims;
using System.Security.Principal;

namespace MyLabLocalizer.Core.Identity
{
    public class AnonymousPrincipal : IPrincipal
    {
        public AnonymousPrincipal()
        {
        }

        IIdentity _identity = new AnonymousIdentity();
        public IIdentity Identity => _identity;

        public bool IsInRole(string role)
        {
            return false;
        }
    }

    public class AuthorizedPrincipal : IPrincipal
    {
        public AuthorizedPrincipal()
        {
        }

        IIdentity _identity = new AuthorizedIdentity();
        public IIdentity Identity => _identity;

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
