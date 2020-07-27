using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ExtendedStringController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAsyncExtendedStringService _extendedStringService;

        public ExtendedStringController(IMapper mapper, IAsyncExtendedStringService extendedStringService)
        {
            _mapper = mapper;
            _extendedStringService = extendedStringService;
        }

        [HttpGet]
        async public Task<IEnumerable<ExtendedString>> Get()
        {
            var extendedStringSearch = new
            {
                ComponentName = "MeasureComponent",
                InternalNamespace = "VASCULAR",
                ISOCoding = "en",
                JobListId = 299,
                ConceptId = 21                
            };

            var result = await _extendedStringService.GetAllAsync(extendedStringSearch.ComponentName, extendedStringSearch.InternalNamespace, extendedStringSearch.ISOCoding, extendedStringSearch.JobListId, extendedStringSearch.ConceptId);
            return await Task.FromResult(_mapper.Map<IEnumerable<ExtendedString>>(result));
        }
    }
}
