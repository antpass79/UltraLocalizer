namespace Globe.Identity.AdministrativeDashboard.Server.Options
{
    public class UserSettingsOptions
    {
        public bool CreateAdmin { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public Role[] Roles { get; set; }
    }

    public class Role
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
