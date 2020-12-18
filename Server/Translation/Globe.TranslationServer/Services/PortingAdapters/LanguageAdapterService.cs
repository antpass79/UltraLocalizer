using AutoMapper;
using Globe.Shared.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
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

        async public Task<IEnumerable<Language>> GetAllAsync()
        {
            var result = await Task.FromResult(_ultraDBStrings.Getlanguage());
            return await Task.FromResult(_mapper.Map<IEnumerable<Language>>(result));
        }

        public Task<Language> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
