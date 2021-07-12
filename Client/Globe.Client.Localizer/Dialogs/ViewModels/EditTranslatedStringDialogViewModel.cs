using Globe.Client.Localizer.Models;
using Globe.Client.Localizer.Services;
using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Services;
using Globe.Client.Platform.Services.Notifications;
using Globe.Client.Platform.ViewModels;
using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Security.Principal;

namespace Globe.Client.Localizer.Dialogs.ViewModels
{
    class EditTranslatedStringDialogViewModel : LocalizeWindowViewModel, IDialogAware
    {
        private readonly IEditStringService _editStringService;
        private readonly ILogService _logService;
        private readonly IPreviewStyleService _previewStyleService;
        private readonly INotificationService _notificationService;

        public EditTranslatedStringDialogViewModel(
            IIdentityStore identityStore,
            IEditStringService editStringService,
            ILogService logService,
            IPreviewStyleService previewStyleService,
            IEventAggregator eventAggregator,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService)
            : base(identityStore, eventAggregator, localizationAppService)
        {
            _editStringService = editStringService;
            _logService = logService;
            _previewStyleService = previewStyleService;
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
        public string Title
        {
            get { return DialogNames.EDIT_TRANSLATED_STRING; }
        }

        private EditableConcept _editableConcept;
        public EditableConcept EditableConcept
        {
            get { return _editableConcept; }
            private set { SetProperty(ref _editableConcept, value); }
        }

        private string _uniqueStringEditable;
        public string UniqueStringEditable
        {
            get { return _uniqueStringEditable; }
            set { SetProperty(ref _uniqueStringEditable, value); }
        }

        private bool _uniqueStringEditableChanged = false;
        public bool UniqueStringEditableChanged
        {
            get { return _uniqueStringEditableChanged; }
            set { SetProperty(ref _uniqueStringEditableChanged, value); }
        }

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ??= new DelegateCommand<string>(parameter =>
            {
                ButtonResult result = ButtonResult.None;

                if (parameter?.ToLower() == "true")
                    result = ButtonResult.OK;
                else if (parameter?.ToLower() == "false")
                    result = ButtonResult.Cancel;

                RaiseRequestClose(new DialogResult(result));
            });

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(async () =>
            {
                SavingBusy = true;

                try
                {
                    var id = EditableConcept.EditableContexts.First().OldStringId;
      
                    await _editStringService.UpdateAsync(new TranslatedString { Id = id, Value = UniqueStringEditable });
                    UniqueStringEditableChanged = false;

                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = Localize[LanguageKeys.Information],
                        Message = Localize[LanguageKeys.Strings_updated],
                        Level = NotificationLevel.Info
                    });
                }
                catch (Exception e)
                {
                    _logService.Exception(e);
                    Console.WriteLine(e.Message);                
                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = Localize[LanguageKeys.Error],
                        Message = Localize[LanguageKeys.Strings_not_updated],
                        Level = NotificationLevel.Error
                    });
                }
                finally
                {
                    SavingBusy = false;
                }
            }, () => CanSave());

        private DelegateCommand _uniqueStringChangeCommand;
        public DelegateCommand UniqueStringChangeCommand =>
            _uniqueStringChangeCommand ??= new DelegateCommand(() =>
            {
                EditableConcept.EditableContexts.ToList().ForEach(item => item.StringEditableValue = UniqueStringEditable);
                UniqueStringEditableChanged = true;
                SaveCommand.RaiseCanExecuteChanged();
            });

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
            EditableConcept = parameters.GetValue<EditableConcept>(DialogParams.EDITABLE_CONCEPT);
            UniqueStringEditable = EditableConcept.EditableContexts.First().StringEditableValue;
        }

        protected override void OnAuthenticationChanged(IPrincipal principal)
        {
            base.OnAuthenticationChanged(principal);
        }

        private bool CanSave()
        {
            return
                !String.IsNullOrWhiteSpace(UniqueStringEditable) && UniqueStringEditableChanged &&
                EditableConcept != null &&
                EditableConcept.EditableContexts.All(item =>
                    !string.IsNullOrWhiteSpace(item.StringEditableValue) &&
                    item.IsPreviewStandardValid &&
                    item.IsPreviewOrangeGrayValid &&
                    item.IsPreviewStandardV2Valid); 
        }
    }
}
