using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/write/[controller]")]
    public class JobListController : Controller
    {
        private readonly IAsyncJobListService _jobListService;

        public JobListController(
            IAsyncJobListService jobListService)
        {
            _jobListService = jobListService;
        }

        [HttpPut]
        [AllowAnonymous]
        async public Task Put([FromBody] NewJobList newJobList)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("newJobList");
            }
            
            await _jobListService.SaveAsync(newJobList);
        }
    }
}
