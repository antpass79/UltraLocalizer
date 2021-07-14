using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
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
