using Globe.Client.Platform.Services;
using Globe.Client.Platofrm.Events;
using Prism.Events;
using System.Collections.Generic;
using System.Security.Principal;

namespace Globe.Client.Platform.ViewModels
{
    public abstract class AuthorizeWindowViewModel : LifecycleWindowViewModel, IAuthorizeWindowViewModel
    {
        private readonly IIdentityStore _identityStore;
        protected IEventAggregator EventAggregator { get; private set; }

        protected AuthorizeWindowViewModel(
            IIdentityStore identityStore,
            IEventAggregator eventAggregator)
        {
            _identityStore = identityStore;
            EventAggregator = eventAggregator;

            EventAggregator.GetEvent<PrincipalChangedEvent>()
                .Subscribe((principal) =>
                {
                    OnAuthenticationChanged(principal);
                });
        }

        public IPrincipal Principal
        {
            get => _identityStore.Principal;
        }

        public IIdentity Identity
        {
            get => _identityStore.Identity;
        }

        public bool IsAuthenticated
        {
            get => _identityStore.IsAuthenticated;
        }

        public IEnumerable<string> UserRoles
        {
            get => _identityStore.UserRoles;
        }

        virtual protected void OnAuthenticationChanged(IPrincipal principal)
        {
            _identityStore.Store(principal);
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Principal)));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Identity)));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(IsAuthenticated)));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(UserRoles)));
        }
    }
}
