namespace Globe.Identity.AdministrativeDashboard.Shared.DTOs
{
    public class CredentialsDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public bool RememberMe { get; set; }
    }
}
