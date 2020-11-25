namespace Globe.Client.Platform.Services
{
    public interface ISettingsService
    {
        string GetLoginBaseAddress();
        string GetLocalizableStringBaseAddress();
        string GetLocalizableStringBaseAddressRead();
        string GetLocalizableStringBaseAddressWrite();
        string GetNotificationHubAddress();
    }
}
