﻿using Globe.BusinessLogic.Repositories;
using Globe.Shared.DTOs;
using Globe.Shared.Utilities;
using Globe.TranslationServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ComponentNamespaceService : IComponentNamespaceService
    {
        private readonly IReadRepository<VJobListConcept> _repository;

        public ComponentNamespaceService(IReadRepository<VJobListConcept> repository)
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