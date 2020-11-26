using System.IO;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncXmlService
    {
        Stream GetZippedContent();
    }
}
