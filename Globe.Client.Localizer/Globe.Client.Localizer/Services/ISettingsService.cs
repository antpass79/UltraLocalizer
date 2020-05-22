namespace Globe.Client.Localizer.Services
{
    public interface ISettingsService
    {
        string GetLoginBaseAddress();
        string GetLocalizableStringBaseAddress();
        string GetLocalizableStringBaseAddressRead();
        string GetLocalizableStringBaseAddressWrite();
    }
}
