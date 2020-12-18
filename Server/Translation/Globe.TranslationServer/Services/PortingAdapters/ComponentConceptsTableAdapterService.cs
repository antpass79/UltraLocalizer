using AutoMapper;
using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ComponentConceptsTableAdapterService : IAsyncComponentNamespaceService
    {
        private readonly IMapper _mapper;
        private readonly LocalizationContext _context;

        public ComponentConceptsTableAdapterService(IMapper mapper, LocalizationContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        async public Task<IEnumerable<ComponentNamespace>> GetAllAsync()
        {
            var result = _context
                .GetAllComponentName()
                .OrderBy(item => item.ComponentNamespace).ToList();
            result.Insert(0, new ComponentConceptsTable
            {
                ComponentNamespace = "all"
            });

            var items = await Task.FromResult(_mapper.Map<IEnumerable<ComponentNamespace>>(result));

            return await Task.FromResult(items);
        }

        public Task<ComponentNamespace> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
