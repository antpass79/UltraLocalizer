using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
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
        async public Task<IEnumerable<StringType>> Get()
        {
            return await _stringTypeService.GetAllAsync();
        }
    }
}
