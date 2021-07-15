using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
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
