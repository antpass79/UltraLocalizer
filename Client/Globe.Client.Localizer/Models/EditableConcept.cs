using System.Collections.ObjectModel;

namespace Globe.Client.Localizer.Models
{
    class EditableConcept
    {
        public EditableConcept(
            int id,
            string componentNamespace,
            string internalNamespace,
            string name,
            string softwareDeveloperComment,
            ObservableCollection<EditableContext> editableContexts)
        {
            Id = id;
            ComponentNamespace = componentNamespace;
            InternalNamespace = internalNamespace;
            Name = name;
            SoftwareDeveloperComment = softwareDeveloperComment;
            EditableContexts = editableContexts;
        }

        public int Id { get; }
        public string ComponentNamespace { get; }
        public string InternalNamespace { get; }
        public string Name { get; }
        public string SoftwareDeveloperComment { get; }
        public ObservableCollection<EditableContext> EditableContexts { get; }

        public string MasterTranslatorComment { get; set; }
        public bool IgnoreTranslation { get; set; }
    }
}
