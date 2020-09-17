using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class StringTypeController : Controller
    {
        private readonly IAsyncStringTypeService _stringTypeService;

        public StringTypeController(IAsyncStringTypeService stringTypeService)
        {
            _stringTypeService = stringTypeService;
        }

        [HttpGet]
        async public Task<IEnumerable<StringTypeDTO>> Get()
        {
            return await _stringTypeService.GetAllAsync();
        }
    }
}
