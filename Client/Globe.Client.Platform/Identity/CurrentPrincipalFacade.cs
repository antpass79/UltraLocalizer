using System;
using System.Security.Principal;

namespace Globe.Client.Platform.Identity
{
    public sealed class CurrentPrincipalFacade : IPrincipal
    {
        public Action<IPrincipal> PrincipalChanged;

        private static readonly CurrentPrincipalFacade _instance = new CurrentPrincipalFacade();

        private CurrentPrincipalFacade()
        {
        }

        public static CurrentPrincipalFacade Instance
        {
            get { return _instance; }
        }

        IPrincipal _principal;
        public IPrincipal Principal
        {
            get => _principal;
            set
            {
                _principal = value;
                PrincipalChanged?.Invoke(_principal);
            }
        }

        public IIdentity Identity
        {
            get { return Principal == null ? null : Principal.Identity; }
        }

        public bool IsInRole(string role)
        {
            return Principal != null && Principal.IsInRole(role);
        }
    }
}
