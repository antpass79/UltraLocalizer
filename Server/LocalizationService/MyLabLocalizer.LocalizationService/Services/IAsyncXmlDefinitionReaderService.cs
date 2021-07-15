using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface IAsyncXmlDefinitionReaderService
    {
        Task<IEnumerable<JobListConcept>> ReadAsync(string folder);
    }
}
