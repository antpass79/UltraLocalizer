using System;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{

    public interface ICompareVersionService
    {
        Task<bool> NewVersionAvailable();
        Task<bool> NewXamlVersionAvailable();
        Task<bool> NewStyleManagerVersionAvailable();
    }
}
