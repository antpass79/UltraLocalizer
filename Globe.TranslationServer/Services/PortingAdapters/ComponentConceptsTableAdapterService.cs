using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ComponentConceptsTableAdapterService : IAsyncComponentConceptsService
    {
        private readonly LocalizationContext _context;

        public ComponentConceptsTableAdapterService(LocalizationContext context)
        {
            _context = context;
        }

        async public Task<IEnumerable<ComponentConceptsTable>> GetAllAsync()
        {
            return await Task.FromResult(_context.GetAllComponentName());
        }

        public Task<ComponentConceptsTable> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
