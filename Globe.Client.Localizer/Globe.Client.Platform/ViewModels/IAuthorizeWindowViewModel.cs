using System.Collections.Generic;
using System.Security.Principal;

namespace Globe.Client.Platform.ViewModels
{
    public interface IAuthorizeWindowViewModel
    {
        IPrincipal Principal { get; }
        bool IsAuthenticated { get; }
        IEnumerable<string> UserRoles { get; }
    }
}