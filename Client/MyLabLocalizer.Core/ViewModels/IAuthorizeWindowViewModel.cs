using System.Collections.Generic;
using System.Security.Principal;

namespace MyLabLocalizer.Core.ViewModels
{
    public interface IAuthorizeWindowViewModel
    {
        IPrincipal Principal { get; }
        bool IsAuthenticated { get; }
        IEnumerable<string> UserRoles { get; }
    }
}