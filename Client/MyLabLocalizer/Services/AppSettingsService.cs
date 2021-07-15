using MyLabLocalizer.Core.Services;
using System.Configuration;

namespace MyLabLocalizer.Services
{
    public class AppSettingsService : ISettingsService
    {
        const string Environment = "Environment";
        const string Schema = "Schema";

        const string ApplicationDownloadAddress = "ApplicationDownloadAddress";
        const string IdentityBaseAddress = "IdentityBaseAddress";

        const string LocalizableStringBaseAddress = "LocalizableStringBaseAddress";
        const string LocalizableStringBaseAddressRead = "LocalizableStringBaseAddressRead";
        const string LocalizableStringBaseAddressWrite = "LocalizableStringBaseAddressWrite";

        const string NotificationHubAddress = "NotificationHubAddress";

        private readonly string _environment;
        private readonly string _schema;

        public AppSettingsService()
        {
            _environment = ConfigurationManager.AppSettings[Environment];
            _schema = ConfigurationManager.AppSettings[Schema];
        }

        public string GetApplicationDownloadAddress()
        {
            return _schema + ConfigurationManager.AppSettings[$"{_environment}{ApplicationDownloadAddress}"];
        }

        public string GetIdentitytBaseAddress()
        {
            return _schema + ConfigurationManager.AppSettings[$"{_environment}{IdentityBaseAddress}"];
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
