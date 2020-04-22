using Prism.Events;
using System.Security.Principal;

namespace Globe.Client.Platofrm.Events
{
    public class PrincipalChangedEvent : PubSubEvent<IPrincipal>
    {
    }
}
