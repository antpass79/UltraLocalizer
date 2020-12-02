using Prism.Mvvm;

namespace Globe.Client.Localizer.Models
{
    class EditableContext : BindableBase
    {
        public EditableContext(string defaultValue, string editableValue, int stringId)
        {
            StringDefaultValue = defaultValue;
            StringEditableValue = editableValue;

            OldStringId = stringId;
        }

        public string Name { get; set; }

        public string ComponentNamespace { get; set; }
        public string InternalNamespace { get; set; }
        public string Concept { get; set; }

        public int Concept2ContextId { get; set; }

        public string StringDefaultValue { get; }
        public int OldStringId { get; }

        bool _linked;
        public bool Linked
        {
            get => _linked;
            private set
            {
                SetProperty<bool>(ref _linked, value);
            }
        }

        StringType _stringType;
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

        bool _isPreviewStandardValid = true;
        public bool IsPreviewStandardValid
        {
            get => _isPreviewStandardValid;
            set
            {
                SetProperty<bool>(ref _isPreviewStandardValid, value);
            }
        }
        bool _isPreviewOrangeGrayValid = true;
        public bool IsPreviewOrangeGrayValid
        {
            get => _isPreviewOrangeGrayValid;
            set
            {
                SetProperty<bool>(ref _isPreviewOrangeGrayValid, value);
            }
        }
        bool _isPreviewStandardV2Valid = true;
        public bool IsPreviewStandardV2Valid
        {
            get => _isPreviewStandardV2Valid;
            set
            {
                SetProperty<bool>(ref _isPreviewStandardV2Valid, value);
            }
        }

        int _stringId;
        public int StringId
        {
            get => _stringId;
            set
            {
                SetProperty<int>(ref _stringId, value);
                Linked = _stringId != 0;
            }
        }
    }
}
