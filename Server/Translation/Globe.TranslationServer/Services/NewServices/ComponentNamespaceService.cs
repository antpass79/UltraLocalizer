using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class ComponentNamespaceService : IAsyncComponentNamespaceService
    {
        private readonly IAsyncReadRepository<LocConceptsTable> _repository;

        public ComponentNamespaceService(IAsyncReadRepository<LocConceptsTable> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<ComponentNamespaceDTO>> GetAllAsync()
        {
            var query = await _repository.QueryAsync();
            var componentNamespaces = query
                .Select(item => item.ComponentNamespace)
                .Distinct()
                .OrderBy(item => item)
                .Select(item => new ComponentNamespaceDTO
                {
                    Description = item
                })
                .ToList();

            componentNamespaces.Insert(0, new ComponentNamespaceDTO
            {
                Description = "all"
            });

            return componentNamespaces;
        }

        public Task<ComponentNamespaceDTO> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
