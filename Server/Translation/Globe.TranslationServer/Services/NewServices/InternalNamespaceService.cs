using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class InternalNamespaceService : IAsyncInternalNamespaceService
    {
        private readonly IAsyncReadRepository<LocConceptsTable> _repository;

        public InternalNamespaceService(IAsyncReadRepository<LocConceptsTable> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<InternalNamespaceDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<InternalNamespaceDTO>> GetAllAsync(string componentNamespace)
        {
            var query = await _repository.QueryAsync();
            var items = query
                .WhereIf(entity => entity.ComponentNamespace == componentNamespace, !string.IsNullOrWhiteSpace(componentNamespace) && componentNamespace.ToLower() != "all")
                .Select(entity => entity.InternalNamespace)
                .Distinct()
                .OrderBy(entity => entity)
                .Select(entity => new InternalNamespaceDTO
                {
                    Description = entity
                })
                .ToList();

            items.Insert(0, new InternalNamespaceDTO
            {
                Description = "all"
            });

            return items;
        }

        public Task<InternalNamespaceDTO> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
