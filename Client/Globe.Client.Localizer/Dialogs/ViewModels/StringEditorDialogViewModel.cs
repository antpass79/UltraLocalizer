using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platform.ViewModels;
using Globe.Shared.DTOs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Globe.Client.Localizer.Dialogs.ViewModels
{
    class StringEditorDialogViewModel : LocalizeWindowViewModel, IDialogAware
    {
        private readonly IEditStringService _editStringService;
        private readonly ILoggerService _loggerService;
        private readonly IPreviewStyleService _previewStyleService;
        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationService _notificationService;

        public StringEditorDialogViewModel(
            IEditStringService editStringService,
            ILoggerService loggerService,
            IPreviewStyleService previewStyleService,
            IEventAggregator eventAggregator,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService)
            : base(eventAggregator, localizationAppService)
        {
            _editStringService = editStringService;
            _loggerService = loggerService;
            _previewStyleService = previewStyleService;
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
        }

        public IPreviewStyleService PreviewStyleService
        {
            get { return _previewStyleService; }
        }

        private bool _savingBusy = false;
        public bool SavingBusy
        {
            get { return _savingBusy; }
            set { SetProperty(ref _savingBusy, value); }
        }

        private bool _searchingBusy = false;
        public bool SearchingBusy
        {
            get { return _searchingBusy; }
            set { SetProperty(ref _searchingBusy, value); }
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

        private Language _language;
        public Language Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }

        private EditableConcept _editableConcept;
        public EditableConcept EditableConcept
        {
            get { return _editableConcept; }
            private set { SetProperty(ref _editableConcept, value); }
        }

        private EditableContext _selectedEditableContext;
        public EditableContext SelectedEditableContext
        {
            get { return _selectedEditableContext; }
            set { SetProperty(ref _selectedEditableContext, value); }
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

        private IEnumerable<Context> _contexts;
        public IEnumerable<Context> Contexts
        {
            get { return _contexts; }
            private set { SetProperty(ref _contexts, value); }
        }

        private Context _selectedContext;
        public Context SelectedContext
        {
            get { return _selectedContext; }
            set { SetProperty(ref _selectedContext, value); }
        }

        private IEnumerable<StringView> _stringViews;
        public IEnumerable<StringView> StringViews
        {
            get { return _stringViews; }
            set { SetProperty(ref _stringViews, value); }
        }

        private StringView _selectedStringView;
        public StringView SelectedStringView
        {
            get { return _selectedStringView; }
            set { SetProperty(ref _selectedStringView, value); }
        }

        IEnumerable<StringType> _stringTypes;
        public IEnumerable<StringType> StringTypes
        {
            get => _stringTypes;
            private set
            {
                SetProperty(ref _stringTypes, value);
            }
        }

        StringType _selectedStringType = StringType.String;
        public StringType SelectedStringType
        {
            get => _selectedStringType;
            set
            {
                SetProperty(ref _selectedStringType, value);
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

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () =>
            {
                SavingBusy = true;

                try
                {
                    await _editStringService.SaveAsync(new SavableConceptModel(Language, EditableConcept));
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = "Information",
                        Message = "String saved",
                        Level = NotificationLevel.Info
                    });
                }
                catch (Exception e)
                {
                    _loggerService.Exception(e);
                    Console.WriteLine(e.Message);                
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = "Error!",
                        Message = "String not saved",
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    SavingBusy = false;
                }
            }, () => CanSave()));

        private DelegateCommand<EditableContext> _linkCommand;
        public DelegateCommand<EditableContext> LinkCommand =>
            _linkCommand ?? (_linkCommand = new DelegateCommand<EditableContext>((editableContext) =>
            {
                editableContext.StringId = this.SelectedStringView.Id;
                editableContext.StringEditableValue = this.SelectedStringView.Value;
                editableContext.StringType = this.SelectedStringView.Type;
            },
            (editableContext) =>
            {
                return this.SelectedStringView != null;
            }));

        private DelegateCommand<EditableContext> _unlinkCommand;
        public DelegateCommand<EditableContext> UnlinkCommand =>
            _unlinkCommand ?? (_unlinkCommand = new DelegateCommand<EditableContext>((editableContext) =>
            {
                editableContext.StringId = 0;
                editableContext.StringEditableValue = null;
                editableContext.StringType = StringType.String;
            },
            (editableContext) =>
            {
                return true;
            }));

        private DelegateCommand<EditableContext> _duplicateCommand;
        public DelegateCommand<EditableContext> DuplicateCommand =>
            _duplicateCommand ?? (_duplicateCommand = new DelegateCommand<EditableContext>((editableContext) =>
            {
                //var index = EditableConcept.EditableContexts.IndexOf(editableContext);
                //EditableConcept.EditableContexts.Remove(editableContext);

                editableContext.StringId = 0;
                editableContext.StringEditableValue = this.SelectedStringView.Value;
                editableContext.StringType = this.SelectedStringView.Type;

                //EditableConcept.EditableContexts.Insert(index, editableContext);
            },
            (editableContext) =>
            {
                return this.SelectedStringView != null;
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
                this.SearchingBusy = true;

                try
                {
                    this.StringViews = await _editStringService.GetStringViewsAsync(new StringViewSearch
                    {
                        StringValue = this.StringValue,
                        ISOCoding = this.Language.IsoCoding,
                        SearchBy = this.SearchBy,
                        FilterBy = this.FilterBy,
                        StringType = this.SelectedStringType,
                        Context = this.SelectedContext.Name
                    });
                }
                catch (Exception e)
                {
                    _loggerService.Exception(e);
                }
                finally
                {
                    this.SearchingBusy = false;
                }
            }));

        private DelegateCommand _localizeChangeCommand;
        public DelegateCommand LocalizeChangeCommand =>
            _localizeChangeCommand ?? (_localizeChangeCommand = new DelegateCommand(() =>
            {
               SaveCommand.RaiseCanExecuteChanged();
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
            EditableConcept.EditableContexts.ToList().ForEach(item => item.PropertyChanged -= EditableContextPropertyChanged);
        }

        async public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            EditableConcept = parameters.GetValue<EditableConcept>(DialogParams.EDITABLE_CONCEPT);
            Language = parameters.GetValue<Language>(DialogParams.LANGUAGE);
            Contexts = await _editStringService.GetContextsAsync();
            StringTypes = await _editStringService.GetStringTypesAsync();
            SelectedContext = this.Contexts.ElementAt(0);

            EditableConcept.EditableContexts.ToList().ForEach(item => item.PropertyChanged += EditableContextPropertyChanged);            
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedStringView))
            {
                LinkCommand.RaiseCanExecuteChanged();
                UnlinkCommand.RaiseCanExecuteChanged();
                DuplicateCommand.RaiseCanExecuteChanged();
            }
            if (args.PropertyName == "StringType")
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private void EditableContextPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args);
        }

        private bool CanSave()
        {
            return EditableConcept != null && EditableConcept.EditableContexts.All(item => item.IsPreviewStandardValid && item.IsPreviewOrangeGrayValid && item.IsPreviewStandardV2Valid);
        }
    }
}
