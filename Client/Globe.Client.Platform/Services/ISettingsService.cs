namespace Globe.Client.Platform.Services
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
