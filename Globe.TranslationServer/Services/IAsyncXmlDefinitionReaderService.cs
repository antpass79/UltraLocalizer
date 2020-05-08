﻿using Globe.TranslationServer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IAsyncXmlDefinitionReaderService
    {
        Task<IEnumerable<StringViewItemDTO>> ReadAsync(string folder);
    }
}
