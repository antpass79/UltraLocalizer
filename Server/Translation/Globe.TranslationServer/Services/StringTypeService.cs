using Globe.Shared.DTOs;
using Globe.TranslationServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class StringTypeService : IAsyncStringTypeService
    {
        private readonly LocalizationContext _context;

        public StringTypeService(LocalizationContext context)
        {
            _context = context;
        }

        async public Task<IEnumerable<StringType>> GetAllAsync()
        {
            var result = _context.LocStringTypes.ToList();
            return await Task.FromResult(result.Select(item => (StringType)item.Id));
        }

        public Task<StringType> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
