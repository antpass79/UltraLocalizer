using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class XmlController : Controller
    {
        private readonly IAsyncXmlZipService _xmlZipService;
        public XmlController(IAsyncXmlZipService xmlZipService)
        {
            _xmlZipService = xmlZipService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var zip = _xmlZipService.Zip();
            return File((zip as MemoryStream).ToArray(), "application/zip", "xml.zip");
        }
    }
}