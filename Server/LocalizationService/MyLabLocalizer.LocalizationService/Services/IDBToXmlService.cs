using MyLabLocalizer.Shared.DTOs;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IDBToXmlService
    {
        Task Generate(string outputFolder, ExportDbFilters exportDbFilters, bool debugMode = true);
    }
}
