using Globe.TranslationServer.DTOs;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncVersionService
    {
        Task<VersionDTO> Get();
    }
}
