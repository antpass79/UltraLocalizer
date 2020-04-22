using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class JobListController : Controller
    {
        private readonly IAsyncJobListService _jobListService;

        public JobListController(IAsyncJobListService jobListService)
        {
            _jobListService = jobListService;
        }

        [HttpGet]
        async public Task<IEnumerable<JobList>> Get(string userName, string ISOCoding)
        {
            return await _jobListService.GetAllAsync(userName, ISOCoding);
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody] JobList jobList)
        {
            await _jobListService.InsertAsync(jobList);
            return await Task.FromResult(Ok());
        }

        [HttpPut]
        async public Task<IActionResult> Put([FromBody] JobList jobList)
        {
            await _jobListService.UpdateAsync(jobList);
            return await Task.FromResult(Ok());
        }

        [HttpDelete]
        async public Task<IActionResult> Delete([FromBody] JobList jobList)
        {
            await _jobListService.DeleteAsync(jobList);
            return await Task.FromResult(Ok());
        }
    }
}
