using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class JobListViewController : Controller
    {
        private readonly IAsyncJobListStatusService _jobListStatusService;

        public JobListViewController(
            IAsyncJobListStatusService jobListStatusService)
        {
            _jobListStatusService = jobListStatusService;
        }

        [HttpGet]
        async public Task<IEnumerable<JobListDTO>> Get([FromBody] JobListSearch search)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("search");
            }
            
            return await _jobListStatusService.GetAsync(search.LanguageId, search.UserName, search.JobListStatus);
        }
    }
}
