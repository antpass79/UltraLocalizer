using System.Collections.Generic;

namespace Globe.TranslationServer.DTOs
{
    public class SavableJobListDTO
    {
        public string JobListName { get; set; }
        public IEnumerable<NotTranslatedConceptViewDTO> NotTranslatedConceptViews { get; set; }
        public ApplicationUserDTO User { get; set; }
        public LanguageDTO Language { get; set; }
    }
}
