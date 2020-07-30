using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class ComponentNamespaceService : IAsyncComponentConceptsService
    {
        private readonly IAsyncReadRepository<LocConceptsTable> _repository;

        public ComponentNamespaceService(IAsyncReadRepository<LocConceptsTable> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<ComponentConceptsTable>> GetAllAsync()
        {
            var query = await _repository.QueryAsync();
            var componentNamespaces = query
                .Select(item => item.ComponentNamespace)
                .Distinct()
                .OrderBy(item => item)
                .Select(item => new ComponentConceptsTable
                {
                    ComponentNamespace = item
                })
                .ToList();

            componentNamespaces.Insert(0, new ComponentConceptsTable
            {
                ComponentNamespace = "all"
            });

            return componentNamespaces;
        }

        public Task<ComponentConceptsTable> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
