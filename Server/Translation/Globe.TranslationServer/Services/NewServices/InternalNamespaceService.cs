using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class InternalNamespaceService : IAsyncInternalConceptsService
    {
        private readonly IAsyncReadRepository<LocConceptsTable> _repository;

        public InternalNamespaceService(IAsyncReadRepository<LocConceptsTable> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<InternalConceptsTable>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<InternalConceptsTable>> GetAllAsync(string componentNamespace)
        {
            var query = await _repository.QueryAsync();
            var items = query
                .WhereIf(entity => entity.ComponentNamespace == componentNamespace, !string.IsNullOrWhiteSpace(componentNamespace) && componentNamespace.ToLower() != "all")
                .Select(entity => entity.InternalNamespace)
                .Distinct()
                .OrderBy(entity => entity)
                .Select(entity => new InternalConceptsTable
                {
                    InternalNamespace = entity
                })
                .ToList();

            items.Insert(0, new InternalConceptsTable
            {
                InternalNamespace = "all"
            });

            return items;
        }

        public Task<InternalConceptsTable> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
