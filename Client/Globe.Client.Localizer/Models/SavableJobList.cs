using System.Collections.Generic;

namespace Globe.Client.Localizer.Models
{
    class SavableJobList
    {
        public SavableJobList(string jobListName, IEnumerable<NotTranslatedConceptView> notTranslatedConceptViews, ApplicationUser user, Language language)
        {
            JobListName = jobListName;
            NotTranslatedConceptViews = notTranslatedConceptViews;
            User = user;
            Language = language;
        }

        public string JobListName { get; }
        public IEnumerable<NotTranslatedConceptView> NotTranslatedConceptViews { get; }
        public ApplicationUser User { get; }
        public Language Language { get; }
    }
}
