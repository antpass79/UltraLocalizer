namespace Globe.Identity.Options
{
    public class GlobePolicy
    {
        public GlobePolicy()
        {
        }
        public GlobePolicy(string name, GlobeClaim[] claims)
        {
            Name = name;
            Claims = claims;
        }
        public GlobePolicy(string name, GlobeClaim[] claims, GlobeRole[] roles)
        {
            Name = name;
            Claims = claims;
            Roles = roles;
        }

        public string Name { get; set; }
        public GlobeClaim[] Claims { get; set; }
        public GlobeRole[] Roles { get; set; }
    }
}
