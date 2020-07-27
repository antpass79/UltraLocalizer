﻿using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncConceptViewProxyService
    {
        Task<IEnumerable<ConceptViewDTO>> GetAllAsync(ConceptViewSearchDTO search);
    }
}