using MyLabLocalizer.Shared.DTOs;
using System.IO;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncXmlZipService
    {
        Stream Zip(ExportDbFilters exportDbFilters);
    }
}
