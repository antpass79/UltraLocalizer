using Globe.BusinessLogic.Repositories;
using Globe.Shared.DTOs;
using Globe.Shared.Utilities;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class InternalNamespaceService : IAsyncInternalNamespaceService
    {
        private readonly IReadRepository<VJobListConcept> _repository;

        public InternalNamespaceService(IReadRepository<VJobListConcept> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<InternalNamespace>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async public Task<IEnumerable<InternalNamespace>> GetAllAsync(string componentNamespace)
        {
            IList<InternalNamespace> items;

            componentNamespace = componentNamespace.ToLower();

            if (componentNamespace != SharedConstants.COMPONENT_NAMESPACE_ALL)
            {
                var query = _repository.Query();
                items = query
                    .WhereIf(entity => entity.ConceptComponentNamespace == componentNamespace, !string.IsNullOrWhiteSpace(componentNamespace))
                    .Select(entity => entity.ConceptInternalNamespace)
                    .Distinct()
                    .OrderBy(entity => entity)
                    .Select(entity => new InternalNamespace
                    {
                        Description = entity
                    })
                    .ToList();

                items.Insert(0, new InternalNamespace
                {
                    Description = SharedConstants.INTERNAL_NAMESPACE_ALL
                });
            }
            else
            {
                items = new List<InternalNamespace>();
                items.Insert(0, new InternalNamespace
                {
                    Description = SharedConstants.INTERNAL_NAMESPACE_ALL
                });
            }

            return await Task.FromResult(items);
        }

        public Task<InternalNamespace> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
