using Globe.Shared.DTOs;

namespace Globe.TranslationServer.DTOs
{
    public class NotTranslateConceptViewSearchDTO
    {
        public ComponentNamespace ComponentNamespace { get; set; }
        public InternalNamespace InternalNamespace { get; set; }
        public Language Language { get; set; }
    }
}
