using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/write/[controller]")]
    public class ConceptController : Controller
    {
        private readonly IAsyncConceptService _conceptService;

        public ConceptController(
            IAsyncConceptService conceptService)
        {
            _conceptService = conceptService;
        }

        [HttpPut]
        async public Task Put([FromBody] SavableConceptModelDTO savableConceptModel)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("savableModel");
            }
            
            await _conceptService.SaveAsync(savableConceptModel);
        }

        [HttpPost]
        async public Task<NewConceptsResult> Post()
        {
            return await _conceptService.CheckNewConceptsAsync();
        }
    }
}
