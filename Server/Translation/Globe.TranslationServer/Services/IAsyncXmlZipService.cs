using System.IO;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncXmlZipService
    {
        Stream Zip();
    }
}
