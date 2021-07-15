using MyLabLocalizer.Shared.DTOs;
using System;

namespace MyLabLocalizer.Models
{
    class JobList
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string OwnerUserName { get; set; }
        public string Name { get; set; }
        public JobListStatus Status { get; set; }
        public JobListStatus NextStatus { get; set; }
        public int TotalConcepts { get; set; }
        public int NumberTranslationsDraft { get; set; }
        public int NumberTranslations { get; set; }
        public int CompletationPercentage
        {
            get
            {
                if (TotalConcepts == 0)
                {
                    return 0;
                }
                else if (NumberTranslations > 0)
                {
                    return Convert.ToInt32((float)NumberTranslations / (float)TotalConcepts * 100);
                }
                else
                {
                    return Convert.ToInt32((float)NumberTranslationsDraft / (float)TotalConcepts * 100);
                }
            }
        }
        public bool IsDraft => NumberTranslationsDraft >= 0 && NumberTranslations == 0;
        public bool IsCompleted => CompletationPercentage == 100;
    }
}
