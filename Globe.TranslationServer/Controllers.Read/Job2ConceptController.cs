using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class Job2ConceptController : Controller
    {
        private readonly IAsyncJob2ConceptService _job2ConceptService;

        public Job2ConceptController(IAsyncJob2ConceptService job2ConceptService)
        {
            _job2ConceptService = job2ConceptService;
        }

        [HttpGet]
        async public Task<IEnumerable<LocJob2Concept>> Get()
        {
            return await _job2ConceptService.GetAllAsync();
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody] LocJob2Concept job2Concept)
        {
            await _job2ConceptService.InsertAsync(job2Concept);
            return await Task.FromResult(Ok());
        }

        [HttpPut]
        async public Task<IActionResult> Put([FromBody] LocJob2Concept job2Concept)
        {
            await _job2ConceptService.UpdateAsync(job2Concept);
            return await Task.FromResult(Ok());
        }

        [HttpDelete]
        async public Task<IActionResult> Delete([FromBody] LocJob2Concept job2Concept)
        {
            await _job2ConceptService.DeleteAsync(job2Concept);
            return await Task.FromResult(Ok());
        }
    }
}
