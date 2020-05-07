using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class StringViewItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAsyncGroupedStringEntityService _groupedStringEntityService;

        public StringViewItemController(IMapper mapper, IAsyncGroupedStringEntityService groupedStringEntityService)
        {
            _mapper = mapper;
            _groupedStringEntityService = groupedStringEntityService;
        }

        [HttpGet]
        async public Task<IEnumerable<StringViewItemDTO>> Get([FromBody] StringItemViewSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }

            var result = await _groupedStringEntityService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.ISOCoding, search.JobListId);
            return await Task.FromResult(_mapper.Map<IEnumerable<StringViewItemDTO>>(result));
        }
    }
}
