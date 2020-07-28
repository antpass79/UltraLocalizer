using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.EFServices
{
    public class LanguageService : IAsyncLanguageService
    {
        private readonly IAsyncReadRepository<LocLanguages> _repository;

        public LanguageService(IAsyncReadRepository<LocLanguages> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<DBLanguage>> GetAllAsync()
        {
            var languages = await _repository.GetAsync();

            return languages
                .Select(language => new DBLanguage
                {
                    IDLanguage = language.Id,
                    DataString = $"{language.LanguageName} ({language.Isocoding})"
                })
                .OrderBy(language => language.DataString);
        }

        public Task<DBLanguage> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
