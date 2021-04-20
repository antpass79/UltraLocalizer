using System;

namespace Globe.Shared.DTOs
{
    public class JobListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string OwnerUserName { get; set; }
        public int StatusId { get; set; }
        public JobListStatus Status { get; set; }
        public int TotalConcepts { get; set; }
        public int NumberTranslationsDraft { get; set; }
        public int NumberTranslations { get; set; }
    }
}
