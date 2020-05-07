using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class InternalConceptsTableAdapterService : IAsyncInternalConceptsService
    {
        private readonly LocalizationContext _context;

        public InternalConceptsTableAdapterService(LocalizationContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<InternalConceptsTable>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<InternalConceptsTable>> GetAllAsync(string componentNamespace)
        {
            var result = _context
                .GetInternalByComponent(componentNamespace)
                .OrderBy(item => item.InternalNamespace).ToList();
            result.Insert(0, new InternalConceptsTable
            {
                InternalNamespace = "all"
            });
            return await Task.FromResult(result);
        }

        public Task<InternalConceptsTable> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
