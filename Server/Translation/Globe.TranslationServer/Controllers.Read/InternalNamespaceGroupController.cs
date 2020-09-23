using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class InternalNamespaceGroupController : Controller
    {
        private readonly IAsyncInternalNamespaceGroupService _internalNamespaceGroupService;

        public InternalNamespaceGroupController(
            IAsyncInternalNamespaceGroupService internalNamespaceGroupService)
        {
            _internalNamespaceGroupService = internalNamespaceGroupService;
        }

        [HttpGet]
        async public Task<IEnumerable<InternalNamespaceGroupDTO>> Get([FromBody] LanguageDTO language)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _internalNamespaceGroupService.GetAllAsync(language);
        }
    }
}
