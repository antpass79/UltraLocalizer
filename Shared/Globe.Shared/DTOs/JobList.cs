namespace Globe.Shared.DTOs
{
    public class JobList
    {
        public int Id { get; set; }   
        public int LanguageId { get; set; }
        public string OwnerUserName { get; set; }
        public string Name { get; set; }
        public JobListStatus Status { get; set; }     
    }
}
