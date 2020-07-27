namespace Globe.Identity.AdministrativeDashboard.Shared.DTOs
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName => $"{LastName} {FirstName}";
    }
}
