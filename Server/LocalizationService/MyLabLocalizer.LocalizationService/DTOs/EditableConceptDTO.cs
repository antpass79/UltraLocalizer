using System.Collections.Generic;

namespace MyLabLocalizer.LocalizationService.DTOs
{
    public class EditableConceptDTO
    {
        public EditableConceptDTO()
        {
        }

        public int Id { get; set; }
        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Name { get; set; }
        public string SoftwareDeveloperComment { get; set; }
        public IEnumerable<EditableContextDTO> EditableContexts { get; set; }
        public string MasterTranslatorComment { get; set; }
        public bool IgnoreTranslation { get; set; }
    }
}