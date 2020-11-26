using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class XmlController : Controller
    {
        private readonly IAsyncXmlService _xmlService;
        public XmlController(IAsyncXmlService xmlService)
        {
            _xmlService = xmlService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var zip = _xmlService.GetZippedContent();
            return File(zip, "application/octet-stream", "xml.zip");
        }
    }
}