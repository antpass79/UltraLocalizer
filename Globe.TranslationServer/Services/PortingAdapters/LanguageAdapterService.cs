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
        private readonly UltraDBStrings _ultraDBStrings;

        public LanguageAdapterService(UltraDBStrings ultraDBStrings)
        {
            _ultraDBStrings = ultraDBStrings;
        }

        async public Task<IEnumerable<DBLanguage>> GetAllAsync()
        {
            return await Task.FromResult(_ultraDBStrings.Getlanguage());
        }

        public Task<DBLanguage> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
