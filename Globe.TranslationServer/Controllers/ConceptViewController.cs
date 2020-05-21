using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class ConceptViewController : Controller
    {
        private readonly IAsyncConceptViewProxyService _conceptViewProxyService;

        public ConceptViewController(
            IAsyncConceptViewProxyService conceptViewProxyService)
        {
            _conceptViewProxyService = conceptViewProxyService;
        }

        [HttpGet]
        async public Task<IEnumerable<ConceptViewDTO>> Get([FromBody] ConceptViewSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _conceptViewProxyService.GetAllAsync(search);
        }
    }
}
