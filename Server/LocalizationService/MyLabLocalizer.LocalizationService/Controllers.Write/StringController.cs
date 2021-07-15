using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/write/[controller]")]
    public class StringController : Controller
    {
        private readonly IAsyncConceptService _conceptService;

        public StringController(
            IAsyncConceptService conceptService)
        {
            _conceptService = conceptService;
        }

        [HttpPut]
        async public Task Put([FromBody] TranslatedString translatedString)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("translatedString");
            }

            await _conceptService.UpdateAsync(translatedString);
        }

    }
}
