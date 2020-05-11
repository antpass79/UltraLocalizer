using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Globe.Client.Localizer.Dialogs.ViewModels
{
    class StringEditorDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IStringEditingService _stringEditingService;

        public StringEditorDialogViewModel(IStringEditingService stringEditingService)
        {
            _stringEditingService = stringEditingService;
        }

        private string _title = DialogNames.STRING_EDITOR;
        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        private string _stringValue = string.Empty;
        public string StringValue
        {
            get { return _stringValue; }
            set { SetProperty(ref _stringValue, value); }
        }

        private string _ISOCoding;
        public string ISOCoding
        {
            get { return _ISOCoding; }
            set { SetProperty(ref _ISOCoding, value); }
        }

        private EditableStringItem _referenceEditableStringItem;
        public EditableStringItem ReferenceEditableStringItem
        {
            get { return _referenceEditableStringItem; }
            private set { SetProperty(ref _referenceEditableStringItem, value); }
        }

        private IEnumerable<EditableStringItem> _editableStringItems;
        public IEnumerable<EditableStringItem> EditableStringItems
        {
            get { return _editableStringItems; }
            set { SetProperty(ref _editableStringItems, value); }
        }

        private ConceptSearchBy _searchBy = ConceptSearchBy.Concept;
        public ConceptSearchBy SearchBy
        {
            get { return _searchBy; }
            set { SetProperty(ref _searchBy, value); }
        }

        private ConceptFilterBy _filterBy = ConceptFilterBy.None;
        public ConceptFilterBy FilterBy
        {
            get { return _filterBy; }
            set { SetProperty(ref _filterBy, value); }
        }

        private IEnumerable<ConceptViewItem> _concepts;
        public IEnumerable<ConceptViewItem> Concepts
        {
            get { return _concepts; }
            set { SetProperty(ref _concepts, value); }
        }

        IEnumerable<StringType> _stringTypes = Enum.GetValues(typeof(StringType)).Cast<StringType>();
        public IEnumerable<StringType> StringTypes
        {
            get => _stringTypes;
            private set
            {
                SetProperty<IEnumerable<StringType>>(ref _stringTypes, value);
            }
        }

        StringType _selectedStringType = StringType.String;
        public StringType SelectedStringType
        {
            get => _selectedStringType;
            set
            {
                SetProperty<StringType>(ref _selectedStringType, value);
            }
        }

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(parameter =>
            {
                ButtonResult result = ButtonResult.None;

                if (parameter?.ToLower() == "true")
                    result = ButtonResult.OK;
                else if (parameter?.ToLower() == "false")
                    result = ButtonResult.Cancel;

                RaiseRequestClose(new DialogResult(result));
            }));

        private DelegateCommand<ConceptSearchBy?> _searchByCommand;
        public DelegateCommand<ConceptSearchBy?> SearchByCommand =>
            _searchByCommand ?? (_searchByCommand = new DelegateCommand<ConceptSearchBy?>((searchBy) =>
            {
                this.SearchBy = searchBy.Value;
            }));

        private DelegateCommand<ConceptFilterBy?> _filterByCommand;
        public DelegateCommand<ConceptFilterBy?> FilterByCommand =>
            _filterByCommand ?? (_filterByCommand = new DelegateCommand<ConceptFilterBy?>((filterBy) =>
            {
                this.FilterBy = filterBy.Value;
            }));

        private DelegateCommand _searchConceptsCommand;
        public DelegateCommand SearchConceptsCommand =>
            _searchConceptsCommand ?? (_searchConceptsCommand = new DelegateCommand(async () =>
            {
                this.Concepts = await _stringEditingService.GetConceptViewItemsAsync(new ConceptViewItemSearch
                {
                    StringValue = this.StringValue,
                    ISOCoding = this.ISOCoding,
                    SearchBy = this.SearchBy,
                    FilterBy = this.FilterBy,
                    StringType = this.SelectedStringType,
                });
            }));

        public event Action<IDialogResult> RequestClose;

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            EditableStringItems = parameters.GetValue<IEnumerable<EditableStringItem>>("editableStringItems");
            ReferenceEditableStringItem = EditableStringItems.ElementAt(0);
            ISOCoding = parameters.GetValue<string>("ISOCoding");
        }
    }
}
