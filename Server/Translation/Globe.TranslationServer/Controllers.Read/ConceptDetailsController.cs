using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
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
        async public Task<ConceptDetailsDTO> Get([FromBody] JobListConcept jobListConcept)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("jobListConcept");
            }
            
            return await _conceptDetailsService.GetAsync(jobListConcept);
        }
    }
}
