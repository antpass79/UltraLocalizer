using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    public class InternalConceptsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAsyncInternalConceptsService _internalConceptsService;

        public InternalConceptsController(IMapper mapper, IAsyncInternalConceptsService internalConceptsService)
        {
            _mapper = mapper;
            _internalConceptsService = internalConceptsService;
        }

        [HttpGet]
        async public Task<IEnumerable<InternalConceptsDTO>> Get()
        {
            var result = await _internalConceptsService.GetAllAsync();
            return await Task.FromResult(_mapper.Map<IEnumerable<InternalConceptsDTO>>(result));
        }
    }
}

