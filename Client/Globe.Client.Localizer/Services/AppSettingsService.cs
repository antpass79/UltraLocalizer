using Globe.Client.Platform.Services;
using System.Configuration;

namespace Globe.Client.Localizer.Services
{
    public class AppSettingsService : ISettingsService
    {
        const string Environment = "Environment";
        const string Schema = "Schema";

        const string LocalizableStringBaseAddress = "LocalizableStringBaseAddress";
        const string LocalizableStringBaseAddressRead = "LocalizableStringBaseAddressRead";
        const string LocalizableStringBaseAddressWrite = "LocalizableStringBaseAddressWrite";
        const string LoginBaseAddress = "LoginBaseAddress";
        const string NotificationHubAddress = "NotificationHubAddress";

        private readonly string _environment;
        private readonly string _schema;

        public AppSettingsService()
        {
            _environment = ConfigurationManager.AppSettings[Environment];
            _schema = ConfigurationManager.AppSettings[Schema];
        }

        public string GetLoginBaseAddress()
        {
            return _schema + ConfigurationManager.AppSettings[$"{_environment}{LoginBaseAddress}"];
        }

        public string GetLocalizableStringBaseAddress()
        {
            return _schema + ConfigurationManager.AppSettings[$"{_environment}{LocalizableStringBaseAddress}"];
        }

        public string GetLocalizableStringBaseAddressRead()
        {
            return _schema + ConfigurationManager.AppSettings[$"{_environment}{LocalizableStringBaseAddressRead}"];
        }

        public string GetLocalizableStringBaseAddressWrite()
        {
            return _schema + ConfigurationManager.AppSettings[$"{_environment}{LocalizableStringBaseAddressWrite}"];
        }

        public string GetNotificationHubAddress()
        {
            return _schema + ConfigurationManager.AppSettings[$"{_environment}{NotificationHubAddress}"];
        }
    }
}
