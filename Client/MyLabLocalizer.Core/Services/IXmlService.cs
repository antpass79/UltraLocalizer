using MyLabLocalizer.Shared.DTOs;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
{
    public interface IXmlService
    {
        Task Download(ExportDbFilters exportDbFilters, string downloadPath = default(string));
    }
}
