using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface IXmlService
    {
        Task Download(string downloadPath = "");
    }
}
