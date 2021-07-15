namespace MyLabLocalizer.Core.Services
{
    public interface ISettingsService
    {
        string GetApplicationDownloadAddress();
        string GetIdentitytBaseAddress();
        string GetLocalizableStringBaseAddress();
        string GetLocalizableStringBaseAddressRead();
        string GetLocalizableStringBaseAddressWrite();
        string GetNotificationHubAddress();
    }
}
