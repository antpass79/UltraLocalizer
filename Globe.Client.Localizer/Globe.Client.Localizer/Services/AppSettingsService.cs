using System.Configuration;

namespace Globe.Client.Localizer.Services
{
    public class AppSettingsService : ISettingsService
    {
        const string LocalizableStringBaseAddress = "LocalizableStringBaseAddress";
        const string LoginBaseAddress = "LoginBaseAddress";

        public string GetLocalizableStringBaseAddress()
        {
            return ConfigurationManager.AppSettings[LocalizableStringBaseAddress];
        }

        public string GetLoginBaseAddress()
        {
            return ConfigurationManager.AppSettings[LoginBaseAddress];
        }
    }
}
