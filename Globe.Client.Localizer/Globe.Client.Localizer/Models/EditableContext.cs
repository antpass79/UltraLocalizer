using Prism.Mvvm;

namespace Globe.Client.Localizer.Models
{
    class EditableContext : BindableBase
    {
        public EditableContext(string defaultValue)
        {
            DefaultValue = defaultValue;
            EditableValue = defaultValue;
        }

        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }
        public string ContextName { get; set; }
        public string DefaultValue { get; }
        public int Concept2ContextId { get; set; }


        public StringType _contextType;
        public StringType ContextType
        {
            get => _contextType;
            set
            {
                SetProperty<StringType>(ref _contextType, value);
            }
        }

        string _editableValue;
        public string EditableValue
        {
            get => _editableValue;
            set
            {
                SetProperty<string>(ref _editableValue, value);
            }
        }

        int _stringId;
        public int StringId
        {
            get => _stringId;
            set
            {
                SetProperty<int>(ref _stringId, value);
            }
        }
    }
}
