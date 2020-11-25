using System;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class CompareVersionService : ICompareVersionService
    {
        #region Data Members

        private readonly IVersionService _remoteVersionService;
        private readonly IVersionService _localVersionService;

        #endregion

        #region Constructors

        public CompareVersionService(IVersionService remoteVersionService, IVersionService localVersionService)
        {
            _remoteVersionService = remoteVersionService;
            _localVersionService = localVersionService;
        }

        async public Task<bool> NewVersionAvailable()
        {
            var remoteVersion = await _remoteVersionService.Get();
            var localVersion = await _localVersionService.Get();

            return
                remoteVersion.StyleManagerVersion != localVersion.StyleManagerVersion ||
                remoteVersion.XamlVersion != localVersion.XamlVersion;
        }

        async public Task<bool> NewStyleManagerVersionAvailable()
        {
            var remoteVersion = await _remoteVersionService.Get();
            var localVersion = await _localVersionService.Get();

            return remoteVersion.StyleManagerVersion != localVersion.StyleManagerVersion;
        }

        async public Task<bool> NewXamlVersionAvailable()
        {
            var remoteVersion = await _remoteVersionService.Get();
            var localVersion = await _localVersionService.Get();

            return remoteVersion.XamlVersion != localVersion.XamlVersion;
        }

        #endregion
    }
}
