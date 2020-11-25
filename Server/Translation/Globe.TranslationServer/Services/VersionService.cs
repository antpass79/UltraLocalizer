using Globe.TranslationServer.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class VersionService : IAsyncVersionService
    {
        public async Task<VersionDTO> Get()
        {
            return await Task<VersionDTO>.FromResult(new VersionDTO { XamlVersion = 1.0f, StyleManagerVersion = 1.0f });
        }
    }
}
