﻿namespace Globe.TranslationServer.DTOs
{
    public class StringViewDTO
    {
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public string Context { get; set; }
        public string Value { get; set; }
        public int StringId { get; set; }
        public StringTypeDTO StringType { get; set; }
        public string SoftwareComment { get; set; }
        public string MasterTranslatorComment { get; set; }
    }
}