using Globe.BusinessLogic.Repositories;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class ExportDbFilterService : IExportDbFilterService
    {
        private readonly IReadRepository<VConceptStringToContext> _conceptStringToContextRepository;
        private readonly IReadRepository<LocLanguage> _languageRepository;

        public ExportDbFilterService(IReadRepository<VConceptStringToContext> conceptStringToContextRepository, IReadRepository<LocLanguage> languageRepository)
        {
            _conceptStringToContextRepository = conceptStringToContextRepository;
            _languageRepository = languageRepository;
        }

        public IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> GetComponentNamespaceGroups(ExportDbFilters exportDbFilters)
        {
            IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> componentNamespaceGroups;

            if(exportDbFilters == null)
            {
                var items = _conceptStringToContextRepository
                .Query(item =>
                    item.StringId != null &&
                    item.ComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD)
                .AsNoTracking()
                .ToList()
                .Select(item =>
                    new Tuple<string, string>(item.ComponentNamespace, item.InternalNamespace));

                componentNamespaceGroups = BuildGroups(items);
            }
            else
            {
                componentNamespaceGroups = exportDbFilters.ComponentNamespaceGroups;
            }

            return componentNamespaceGroups;
        }

        private IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> BuildGroups(IEnumerable<Tuple<string, string>> items)
        {
            var componentNamespaceGroups = new List<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>>();

            var groups = items
                .GroupBy(item => item.Item1)
                .OrderBy(item => item.Key);

            foreach (var group in groups)
            {
                componentNamespaceGroups.Add(new ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>
                {
                    ComponentNamespace = new ComponentNamespace { Description = group.Key },
                    InternalNamespaces = group
                    .GroupBy(item => item.Item2)
                    .Select(item => new InternalNamespace { Description = item.Key })
                    .OrderBy(item => item.Description)
                });
            }

            return componentNamespaceGroups;
        }

        public IEnumerable<Language> GetLanguages(ExportDbFilters exportDbFilters)
        {
            IEnumerable<Language> languages;

            if (exportDbFilters == null)
            {
                languages = _languageRepository
                    .Get()
                    .Select(item => new Language 
                    { 
                        IsoCoding = item.Isocoding,
                        Name = item.LanguageName,
                        Id = item.Id,
                        Description = item.LanguageName
                    });
            }
            else
            {
                languages = exportDbFilters.Languages.Select(item => new Language
                {
                    IsoCoding = item.IsoCoding,
                    Name = item.Name,
                    Id = item.Id,
                    Description = item.Description
                });
            }

            return languages;
        }
    }
}
