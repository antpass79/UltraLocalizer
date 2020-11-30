using Globe.Client.Platform.Models;
using System;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface ICompareVersionService
    {
        VersionDTO CurrentVersion { get; }
        VersionDTO AvailableVersion { get; }

        bool NewVersionAvailable();
        bool NewStyleVersionAvailable();
        bool NewApplicationVersionAvailable();
    }
}
