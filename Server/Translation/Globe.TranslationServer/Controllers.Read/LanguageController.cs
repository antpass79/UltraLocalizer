using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class LanguageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAsyncLanguageService _languageService;

        public LanguageController(IMapper mapper, IAsyncLanguageService languageService)
        {
            _mapper = mapper;
            _languageService = languageService;
        }

        [HttpGet]
        async public Task<IEnumerable<LanguageDTO>> Get()
        {
            var result = await _languageService.GetAllAsync();
            return await Task.FromResult(_mapper.Map<IEnumerable<LanguageDTO>>(result));
        }
    }
}
