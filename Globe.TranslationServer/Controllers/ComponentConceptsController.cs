using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    public class ComponentConceptsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAsyncComponentConceptsService _componentConceptsService;

        public ComponentConceptsController(IMapper mapper, IAsyncComponentConceptsService componentConceptsService)
        {
            _mapper = mapper;
            _componentConceptsService = componentConceptsService;
        }

        [HttpGet]
        async public Task<IEnumerable<ComponentConceptsDTO>> Get()
        {
            var result = await _componentConceptsService.GetAllAsync();
            return await Task.FromResult(_mapper.Map<IEnumerable<ComponentConceptsDTO>>(result));
        }
    }
}
