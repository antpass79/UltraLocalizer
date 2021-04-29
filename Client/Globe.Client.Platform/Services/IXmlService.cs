using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface IXmlService
    {
        Task Download(Shared.DTOs.ExportDbFilters exportDbFilters, string downloadPath = default(string));
    }
}
