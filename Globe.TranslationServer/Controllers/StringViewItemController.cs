using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class StringViewItemController : Controller
    {
        private readonly IAsyncStringViewItemProxyService _stringViewItemProxyService;

        public StringViewItemController(
            IAsyncStringViewItemProxyService stringViewItemProxyService)
        {
            _stringViewItemProxyService = stringViewItemProxyService;
        }

        [HttpGet]
        async public Task<IEnumerable<StringViewItemDTO>> Get([FromBody] StringViewItemSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _stringViewItemProxyService.GetAllAsync(search);
        }
    }
}
