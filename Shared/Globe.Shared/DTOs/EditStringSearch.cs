namespace Globe.Shared.DTOs
{
    public class EditStringSearch
    {
        public int StringId { get; set; }
        public int LanguageId { get; set; }
        public string Concept { get; set; }
        public string LocalizedString { get; set; }
    }
}
