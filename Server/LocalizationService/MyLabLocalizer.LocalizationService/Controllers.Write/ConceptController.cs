using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/write/[controller]")]
    public class ConceptController : Controller
    {
        private readonly IAsyncConceptService _conceptService;

        public ConceptController(
            IAsyncConceptService conceptService)
        {
            _conceptService = conceptService;
        }

        [HttpPut]
        async public Task Put([FromBody] SavableConceptModelDTO savableConceptModel)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("savableModel");
            }
            
            await _conceptService.SaveAsync(savableConceptModel);
        }

        [HttpPost]
        async public Task<NewConceptsResult> Post()
        {
            return await _conceptService.CheckNewConceptsAsync();
        }
    }
}
