using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class StyleController : Controller
    {
        private readonly IAsyncStyleService _styleService;
        public StyleController(IAsyncStyleService styleService)
        {
            _styleService = styleService;
        }

        [HttpGet]
        public async Task<string> Get(string filePath)
        {
            return await _styleService.GetFileContent(filePath);
        }
    }
}