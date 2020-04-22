using Globe.TranslationServer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class LanguageAdapterService : IAsyncLanguageService
    {
        public LanguageAdapterService()
        {
        }

        public Task<IEnumerable<LocLanguages>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<LocLanguages> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
