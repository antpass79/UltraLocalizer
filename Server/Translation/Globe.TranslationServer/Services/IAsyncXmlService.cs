using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncXmlService
    {
        Task<byte[]> GetZippedContent();
    }
}
