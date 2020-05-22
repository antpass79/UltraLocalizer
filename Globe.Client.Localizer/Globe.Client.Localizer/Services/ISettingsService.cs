namespace Globe.Client.Localizer.Services
{
    public interface ISettingsService
    {
        string GetLoginBaseAddress();
        string GetLocalizableStringBaseAddressRead();
        string GetLocalizableStringBaseAddressWrite();
    }
}
