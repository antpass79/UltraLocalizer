using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ComponentNamespaceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAsyncComponentConceptsService _componentConceptsService;

        public ComponentNamespaceController(IMapper mapper, IAsyncComponentConceptsService componentConceptsService)
        {
            _mapper = mapper;
            _componentConceptsService = componentConceptsService;
        }

        [HttpGet]
        async public Task<IEnumerable<ComponentNamespaceDTO>> Get()
        {
            var result = await _componentConceptsService.GetAllAsync();
            return await Task.FromResult(_mapper.Map<IEnumerable<ComponentNamespaceDTO>>(result));
        }
    }
}
