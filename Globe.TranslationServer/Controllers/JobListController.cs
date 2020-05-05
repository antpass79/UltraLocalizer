﻿using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Controllers
{
    [Route("api/[controller]")]
    public class JobListController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAsyncJobListService _jobListService;

        public JobListController(IMapper mapper, IAsyncJobListService jobListService)
        {
            _mapper = mapper;
            _jobListService = jobListService;
        }

        [HttpGet]
        async public Task<IEnumerable<JobListDTO>> Get([FromBody] JobListSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }

            var result = await _jobListService.GetAllAsync(search.UserName, search.coding);
            return await Task.FromResult(_mapper.Map<IEnumerable<JobListDTO>>(result));
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
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
