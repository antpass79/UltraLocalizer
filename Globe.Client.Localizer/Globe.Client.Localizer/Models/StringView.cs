namespace Globe.Client.Localizer.Models
{
    class StringView
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public string Context { get; set; }
        public string StringValue { get; set; }
        public int StringId { get; set; }
        public StringType StringType { get; set; }
        public string SoftwareComment { get; set; }
        public string MasterTranslatorComment { get; set; }
    }
}
