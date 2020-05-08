using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class ConceptViewItemController : Controller
    {
        private readonly IAsyncConceptViewItemProxyService _conceptViewItemProxyService;

        public ConceptViewItemController(
            IAsyncConceptViewItemProxyService conceptViewItemProxyService)
        {
            _conceptViewItemProxyService = conceptViewItemProxyService;
        }

        [HttpGet]
        async public Task<IEnumerable<ConceptViewItemDTO>> Get([FromBody] ConceptViewItemSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _conceptViewItemProxyService.GetAllAsync(search);
        }
    }
}
