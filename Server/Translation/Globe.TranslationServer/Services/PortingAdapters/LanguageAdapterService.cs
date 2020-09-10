using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class LanguageAdapterService : IAsyncLanguageService
    {
        private readonly IMapper _mapper;
        private readonly UltraDBStrings _ultraDBStrings;

        public LanguageAdapterService(IMapper mapper, UltraDBStrings ultraDBStrings)
        {
            _mapper = mapper;
            _ultraDBStrings = ultraDBStrings;
        }

        async public Task<IEnumerable<LanguageDTO>> GetAllAsync()
        {
            var result = await Task.FromResult(_ultraDBStrings.Getlanguage());
            return await Task.FromResult(_mapper.Map<IEnumerable<LanguageDTO>>(result));
        }

        public Task<LanguageDTO> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
