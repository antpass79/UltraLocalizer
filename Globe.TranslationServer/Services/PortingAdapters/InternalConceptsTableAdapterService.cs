using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Collections.Generic;
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
            return await Task.FromResult(_context.GetInternalByComponent(componentNamespace));
        }

        public Task<InternalConceptsTable> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
