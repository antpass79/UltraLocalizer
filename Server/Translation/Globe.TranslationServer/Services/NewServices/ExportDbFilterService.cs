using Globe.BusinessLogic.Repositories;
using Globe.Shared.DTOs;
using Globe.TranslationServer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Services.NewServices
{
    public class ExportDbFilterService : IExportDbFilterService
    {
        private readonly IReadRepository<VLocalization> _localizationViewRepository;
        private readonly IReadRepository<LocLanguage> _languageRepository;

        public ExportDbFilterService(IReadRepository<VLocalization> localizationViewRepository, IReadRepository<LocLanguage> languageRepository)
        {
            _localizationViewRepository = localizationViewRepository;
            _languageRepository = languageRepository;
        }

        public IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> GetComponentNamespaceGroups(ExportDbFilters exportDbFilters)
        {
            IEnumerable<ComponentNamespaceGroup<ComponentNamespace, InternalNamespace>> componentNamespaceGroups;

            if(exportDbFilters == null)
            {
                componentNamespaceGroups = _localizationViewRepository
                .Get()
                .GroupBy(item => item.ConceptComponentNamespace)
                .Select(group => new ComponentNamespaceGroup 
                { 
                    ComponentNamespace = new ComponentNamespace
                    {
                        Description = group.Key
                    },
                    InternalNamespaces = group.Select(item => new InternalNamespace
                    {
                        Description = item.ConceptInternalNamespace
                    })
                });               
            }
            else
            {
                componentNamespaceGroups = exportDbFilters.ComponentNamespaceGroups;
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
