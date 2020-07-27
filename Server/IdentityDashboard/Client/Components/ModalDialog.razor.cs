using Microsoft.AspNetCore.Components;

namespace Globe.Identity.AdministrativeDashboard.Client.Components
{
    public enum ButtonType
    {
        OK = 0,
        Cancel = 1,
        Yes = 2,
        No = 3
    }

    public class ModalDialogDataModel : ComponentBase
    {
        [Parameter]
        public EventCallback<ButtonType> OnButtonClick { get; set; }

        [Parameter]
        public bool IsOpen { get; set; } = false;
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Message { get; set; }
        [Parameter]
        public bool ShowOK { get; set; } = false;
        [Parameter]
        public bool ShowCancel { get; set; } = false;
        [Parameter]
        public bool ShowYes { get; set; } = false;
        [Parameter]
        public bool ShowNo { get; set; } = false;

        protected void HandleButton(ButtonType buttonType)
        {
            OnButtonClick.InvokeAsync(buttonType);
        }
    }
}