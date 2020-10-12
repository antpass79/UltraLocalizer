using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Extensions;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class JobItemController : ControllerBase
    {
        private readonly IAsyncJobItemService _jobItemService;

        public JobItemController(IAsyncJobItemService jobItemService)
        {
            _jobItemService = jobItemService;
        }

        [HttpGet]
        async public Task<IEnumerable<JobItemDTO>> Get([FromBody] JobItemSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }

            return await _jobItemService.GetAllAsync(search.UserName, search.ISOCoding, this.User.IsMasterTranslator());
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody] JobItemDTO jobItem)
        {
            await _jobItemService.InsertAsync(jobItem);
            return await Task.FromResult(Ok());
        }

        [HttpPut]
        async public Task<IActionResult> Put([FromBody] JobItemDTO jobItem)
        {
            await _jobItemService.UpdateAsync(jobItem);
            return await Task.FromResult(Ok());
        }

        [HttpDelete]
        async public Task<IActionResult> Delete([FromBody] JobItemDTO jobItem)
        {
            await _jobItemService.DeleteAsync(jobItem);
            return await Task.FromResult(Ok());
        }
    }
}
