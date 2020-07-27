namespace Globe.Identity.Options
{
    public class GlobeRole
    {
        public GlobeRole()
        {
        }

        public GlobeRole(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
