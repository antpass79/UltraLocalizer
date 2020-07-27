using System.Security.Principal;

namespace Globe.Client.Platform.Identity
{
    public class AnonymousIdentity : IIdentity
    {
        public AnonymousIdentity()
        {
        }

        public string AuthenticationType => "Anonymous";

        public bool IsAuthenticated => false;

        public string Name => string.Empty;
    }

    public class AuthorizedIdentity : IIdentity
    {
        public AuthorizedIdentity()
        {
        }

        public string AuthenticationType => "Authorized";

        public bool IsAuthenticated => true;

        public string Name => "Authorizer User";
    }
}
