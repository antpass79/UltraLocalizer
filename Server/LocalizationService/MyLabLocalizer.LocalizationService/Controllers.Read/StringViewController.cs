﻿using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Controllers
{
    [Route("api/read/[controller]")]
    public class StringViewController : Controller
    {
        private readonly IAsyncStringViewProxyService _stringViewProxyService;

        public StringViewController(
            IAsyncStringViewProxyService stringViewProxyService)
        {
            _stringViewProxyService = stringViewProxyService;
        }

        [HttpGet]
        async public Task<IEnumerable<StringViewDTO>> Get([FromBody] StringViewSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception("search");
            }
            
            return await _stringViewProxyService.GetAllAsync(search);
        }
    }
}
