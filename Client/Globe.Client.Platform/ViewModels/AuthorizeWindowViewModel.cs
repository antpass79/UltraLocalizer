using Globe.Client.Platform.Extensions;
using Globe.Client.Platform.Identity;
using Globe.Client.Platofrm.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Security.Principal;

namespace Globe.Client.Platform.ViewModels
{
    public abstract class AuthorizeWindowViewModel : BindableBase, IAuthorizeWindowViewModel
    {
        IEventAggregator EventAggregator { get; }

        protected AuthorizeWindowViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            EventAggregator.GetEvent<PrincipalChangedEvent>()
                .Subscribe((principal) =>
                {
                    OnAuthenticationChanged(principal);
                });
        }

        IPrincipal _principal = new AnonymousPrincipal();
        public IPrincipal Principal
        {
            get => _principal;
            private set
            {
                SetProperty<IPrincipal>(ref _principal, value ?? new AnonymousPrincipal());
            }
        }

        IIdentity _identity = new AnonymousIdentity();
        public IIdentity Identity
        {
            get => _identity;
            private set
            {
                SetProperty<IIdentity>(ref _identity, value ?? new AnonymousIdentity());
            }
        }

        bool _isAuthenticated = false;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            private set
            {
                SetProperty<bool>(ref _isAuthenticated, value);
            }
        }

        IEnumerable<string> _userRoles;
        public IEnumerable<string> UserRoles
        {
            get => _userRoles;
            private set
            {
                SetProperty<IEnumerable<string>>(ref _userRoles, value);
            }
        }

        virtual protected void OnAuthenticationChanged(IPrincipal principal)
        {
            this.Principal = principal;
            this.Identity = principal.Identity;
            this.IsAuthenticated = principal.Identity.IsAuthenticated;
            this.UserRoles = principal.Identity.GetRoles();
        }
    }
}
