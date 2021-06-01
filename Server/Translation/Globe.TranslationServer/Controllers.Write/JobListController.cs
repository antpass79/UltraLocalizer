using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/write/[controller]")]
    public class JobListController : Controller
    {
        private readonly IJobListService _jobListService;

        public JobListController(
            IJobListService jobListService)
        {
            _jobListService = jobListService;
        }

        [HttpPut]
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
