using Globe.Shared.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/read/[controller]")]
    public class ConceptViewController : Controller
    {
        private readonly IJobListService _jobListService;

        public ConceptViewController(
            IJobListService jobListService)
        {
            _jobListService = jobListService;
        }

        [HttpGet]
        async public Task<IEnumerable<JobListConcept>> Get([FromBody] JobListConceptSearch search)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("search");
            }
            
            return await _jobListService.GetAllAsync(search.ComponentNamespace, search.InternalNamespace, search.LanguageId, search.JobListId);
        }
    }
}
