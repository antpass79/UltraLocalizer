using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class LanguageService : IAsyncLanguageService
    {
        private readonly IAsyncReadRepository<LocLanguages> _repository;

        public LanguageService(IAsyncReadRepository<LocLanguages> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<LanguageDTO>> GetAllAsync()
        {
            var query = await _repository.QueryAsync();
            return query
                .Select(language => new LanguageDTO
                {
                    Id = language.Id,
                    Name = language.LanguageName,
                    Description = $"{language.LanguageName} ({language.Isocoding})",
                    IsoCoding = language.Isocoding
                })
                .AsEnumerable()
                .OrderBy(language => language.Description);
        }

        public Task<LanguageDTO> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
