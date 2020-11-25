using Globe.Client.Platform.Services;
using System.Configuration;

namespace Globe.Client.Localizer.Services
{
    public class AppSettingsService : ISettingsService
    {
        const string LocalizableStringBaseAddress = "LocalizableStringBaseAddress";
        const string LocalizableStringBaseAddressRead = "LocalizableStringBaseAddressRead";
        const string LocalizableStringBaseAddressWrite = "LocalizableStringBaseAddressWrite";
        const string LoginBaseAddress = "LoginBaseAddress";
        const string NotificationHubAddress = "NotificationHubAddress";

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

        public string GetNotificationHubAddress()
        {
            return ConfigurationManager.AppSettings[NotificationHubAddress];
        }
    }
}
