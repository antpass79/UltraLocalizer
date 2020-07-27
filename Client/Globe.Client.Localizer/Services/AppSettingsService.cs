using System.Configuration;

namespace Globe.Client.Localizer.Services
{
    public class AppSettingsService : ISettingsService
    {
        const string LocalizableStringBaseAddress = "LocalizableStringBaseAddress";
        const string LocalizableStringBaseAddressRead = "LocalizableStringBaseAddressRead";
        const string LocalizableStringBaseAddressWrite = "LocalizableStringBaseAddressWrite";
        const string LoginBaseAddress = "LoginBaseAddress";

        public string GetLoginBaseAddress()
        {
            return ConfigurationManager.AppSettings[LoginBaseAddress];
        }

        public string GetLocalizableStringBaseAddress()
        {
            return ConfigurationManager.AppSettings[LocalizableStringBaseAddress];
        }

        public string GetLocalizableStringBaseAddressRead()
        {
            return ConfigurationManager.AppSettings[LocalizableStringBaseAddressRead];
        }

        public string GetLocalizableStringBaseAddressWrite()
        {
            return ConfigurationManager.AppSettings[LocalizableStringBaseAddressWrite];
        }
    }
}
