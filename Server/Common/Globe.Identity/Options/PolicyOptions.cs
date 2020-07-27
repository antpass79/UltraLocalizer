using Globe.Identity.Security;

namespace Globe.Identity.Options
{
    public class PolicyOptions
    {
        public GlobePolicy[] Policies { get; set; }
        public GlobeRole[] Roles { get; set; }
    }
}
