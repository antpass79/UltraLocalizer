using Globe.BusinessLogic.Repositories;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class ConceptTranslatedComponentNamespaceService : IAsyncConceptTranslatedComponentNamespaceService
    {
        private readonly IReadRepository<VTranslatedConcept> _repository;

        public ConceptTranslatedComponentNamespaceService(IReadRepository<VTranslatedConcept> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<ComponentNamespace>> GetAllAsync()
        {
            var componentNamespaces = _repository.Query()
                .Select(item => item.ConceptComponentNamespace)
                .Distinct()
                .OrderBy(item => item)
                .Select(item => new ComponentNamespace
                {
                    Description = item
                })
                .ToList();

            componentNamespaces.Insert(0, new ComponentNamespace
            {
                Description = SharedConstants.COMPONENT_NAMESPACE_ALL
            });

            return await Task.FromResult(componentNamespaces);
        }

        public Task<ComponentNamespace> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
