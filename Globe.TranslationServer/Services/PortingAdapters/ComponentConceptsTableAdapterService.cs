using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = _context
                .GetAllComponentName()
                .OrderBy(item => item.ComponentNamespace).ToList();
            result.Insert(0, new ComponentConceptsTable
            {
                ComponentNamespace = "all"
            });
            return await Task.FromResult(result);
        }

        public Task<ComponentConceptsTable> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
