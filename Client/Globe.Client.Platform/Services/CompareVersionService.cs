using Globe.Client.Platform.Models;
using System;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class CompareVersionService : ICompareVersionService
    {
        #region Data Members

        private VersionDTO _remoteVersion;
        private VersionDTO _localVersion;

        #endregion

        #region Constructors

        public CompareVersionService(IVersionService remoteVersionService, IVersionService localVersionService)
        {
            Task.Run(async () =>
            {
                _remoteVersion = await remoteVersionService.Get();
                _localVersion = await localVersionService.Get();
            }).Wait();
        }

        #endregion

        #region Properties

        public VersionDTO CurrentVersion => _localVersion;
        public VersionDTO AvailableVersion => _remoteVersion;

        #endregion

        #region Public Functions

        public bool NewVersionAvailable()
        {
            return
                _remoteVersion.ApplicationVersion != _localVersion.ApplicationVersion ||
                _remoteVersion.StyleVersion != _localVersion.StyleVersion;
        }

        public bool NewApplicationVersionAvailable()
        {
            return _remoteVersion.ApplicationVersion != _localVersion.ApplicationVersion;
        }

        public bool NewStyleVersionAvailable()
        {
            return _remoteVersion.StyleVersion != _localVersion.StyleVersion;
        }

        #endregion
    }
}
