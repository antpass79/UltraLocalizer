namespace Globe.Client.Localizer.Models
{
    class EditableStringItem
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public string ContextName { get; set; }
        public StringType ContextType { get; set; }
        public string ContextValue { get; set; }
        public int StringId { get; set; }
        public int Concept2ContextId { get; set; }
    }
}
