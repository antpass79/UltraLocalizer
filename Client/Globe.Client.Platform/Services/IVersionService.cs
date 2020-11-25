using Globe.Client.Platform.Models;
using System;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{

    public interface IVersionService
    {
        Task<VersionDTO> Get();
    }
}
