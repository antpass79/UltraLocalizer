using AutoMapper;
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
    public class InternalConceptsTableAdapterService : IAsyncInternalNamespaceService
    {
        private readonly IMapper _mapper;
        private readonly LocalizationContext _context;

        public InternalConceptsTableAdapterService(IMapper mapper, LocalizationContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task<IEnumerable<InternalNamespaceDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<InternalNamespaceDTO>> GetAllAsync(string componentNamespace)
        {
            var result = _context
                .GetInternalByComponent(componentNamespace)
                .OrderBy(item => item.InternalNamespace).ToList();
            result.Insert(0, new InternalConceptsTable
            {
                InternalNamespace = "all"
            });

            var items = await Task.FromResult(_mapper.Map<IEnumerable<InternalNamespaceDTO>>(result));

            return await Task.FromResult(items);
        }

        public Task<InternalNamespaceDTO> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
