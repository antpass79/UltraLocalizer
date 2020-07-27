using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ConceptDetailsController : Controller
    {
        private readonly IAsyncConceptDetailsService _conceptDetailsService;

        public ConceptDetailsController(
            IAsyncConceptDetailsService conceptDetailsService)
        {
            _conceptDetailsService = conceptDetailsService;
        }

        [HttpGet]
        async public Task<ConceptDetailsDTO> Get([FromBody] ConceptViewDTO conceptView)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("concept");
            }
            
            return await _conceptDetailsService.GetAsync(conceptView);
        }
    }
}
