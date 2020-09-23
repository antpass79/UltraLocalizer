using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
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
        async public Task Put([FromBody] SavableJobListDTO savableJobList)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("savableModel");
            }
            
            await _jobListService.SaveAsync(savableJobList);
        }
    }
}
