using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class InternalNamespaceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAsyncInternalConceptsService _internalConceptsService;

        public InternalNamespaceController(IMapper mapper, IAsyncInternalConceptsService internalConceptsService)
        {
            _mapper = mapper;
            _internalConceptsService = internalConceptsService;
        }

        [HttpGet]
        async public Task<IEnumerable<InternalNamespaceDTO>> Get([FromQuery] string componentNamespace)
        {
            var result = await _internalConceptsService.GetAllAsync(componentNamespace);
            return await Task.FromResult(_mapper.Map<IEnumerable<InternalNamespaceDTO>>(result));
        }
    }
}

