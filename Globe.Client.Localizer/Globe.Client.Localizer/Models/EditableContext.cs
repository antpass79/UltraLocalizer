using Prism.Mvvm;

namespace Globe.Client.Localizer.Models
{
    class EditableContext : BindableBase
    {
        public EditableContext(string defaultValue)
        {
            StringDefaultValue = defaultValue;
            StringEditableValue = defaultValue;
        }

        public string Name { get; set; }

        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }

        public int Concept2ContextId { get; set; }

        public string StringDefaultValue { get; }


        public StringType _stringType;
        public StringType StringType
        {
            get => _stringType;
            set
            {
                SetProperty<StringType>(ref _stringType, value);
            }
        }

        string _stringEditableValue;
        public string StringEditableValue
        {
            get => _stringEditableValue;
            set
            {
                SetProperty<string>(ref _stringEditableValue, value);
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
