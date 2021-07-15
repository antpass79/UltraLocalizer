using Globe.BusinessLogic.Repositories;
using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class NotTranslatedConceptViewService : IAsyncNotTranslatedConceptViewService
    {
        private readonly IReadRepository<VConceptStringToContext> _conceptStringToContextRepository;
        private readonly IReadRepository<VStringsToContext> _stringsToContextRepository;

        public NotTranslatedConceptViewService(
            IReadRepository<VConceptStringToContext> conceptStringToContextRepository,
            IReadRepository<VStringsToContext> stringsToContextRepository)
        {
            _conceptStringToContextRepository = conceptStringToContextRepository;
            _stringsToContextRepository = stringsToContextRepository;
        }

        async public Task<IEnumerable<NotTranslatedConceptView>> GetAllAsync(NotTranslatedConceptViewSearch search)
        {
            try
            {
                var result = search.Language.IsoCoding == SharedConstants.LANGUAGE_EN
                    ?
                    GetConceptToContextsByEnglish(search.ComponentNamespace, search.InternalNamespace)
                    :
                    GetConceptToContextsByLanguage(search.ComponentNamespace, search.InternalNamespace, search.Language);

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during ComponentNamespaceGroupService.GetAllAsync({search.ComponentNamespace.Description}, {search.InternalNamespace.Description}, {search.Language.IsoCoding}), {e.Message}");
            }
        }

        #region Private Functions

        IEnumerable<NotTranslatedConceptView> GetConceptToContextsByEnglish(ComponentNamespace componentNamespace, InternalNamespace internalNamespace)
        {
            var items = _conceptStringToContextRepository
                .Query(item =>
                    item.StringId == null &&
                    item.ComponentNamespace == componentNamespace.Description &&
                    item.InternalNamespace == internalNamespace.Description)
                .AsNoTracking()
                .ToList()
                .Select(item =>
                    new Tuple<int, string, string, string, string, int>(item.Id, item.ComponentNamespace, item.InternalNamespace, item.LocalizationId, item.ContextName, item.Idconcept2Context));

            return BuildGroups(items);
        }

        IEnumerable<NotTranslatedConceptView> GetConceptToContextsByLanguage(ComponentNamespace componentNamespace, InternalNamespace internalNamespace, Language language)
        {
            var componentNamespaceGroups = new List<ComponentNamespaceGroup>();

            var subQuery = _stringsToContextRepository
                .Query(item =>
                    item.Isocoding == language.IsoCoding &&
                    item.ComponentNamespace == componentNamespace.Description &&
                    item.InternalNamespace == internalNamespace.Description)
                .Select(item => item.Id);

            var items = _stringsToContextRepository
                .Query(item =>
                    item.Ignore.HasValue &&
                    !item.Ignore.Value &&
                    item.Isocoding == SharedConstants.LANGUAGE_EN &&
                    item.ComponentNamespace == componentNamespace.Description &&
                    item.InternalNamespace == internalNamespace.Description &&
                    !subQuery.Contains(item.Id))
                .AsNoTracking()
                .ToList()
                .Select(item =>
                    new Tuple<int, string, string, string, string, int>(item.ConceptId, item.ComponentNamespace, item.InternalNamespace, item.LocalizationId, item.ContextName, item.Id));

            return BuildGroups(items);
        }

        private IEnumerable<NotTranslatedConceptView> BuildGroups(IEnumerable<Tuple<int, string, string, string, string, int>> items)
        {
            var notTranslatedConceptToContextGroups = new List<NotTranslatedConceptView>();

            var groups = items
                .GroupBy(item => item.Item1)
                .OrderBy(item => item.First().Item2)
                .ThenBy(item => item.First().Item3)
                .ThenBy(item => item.First().Item4);

            foreach (var group in groups)
            {
                notTranslatedConceptToContextGroups.Add(new NotTranslatedConceptView
                {
                    ComponentNamespace = new ComponentNamespace { Description = group.First().Item2 },
                    InternalNamespace = new InternalNamespace { Description = group.First().Item3 },
                    Name = group.First().Item4,
                    ContextViews = group.Select(item => new JobListContext
                    {
                        Name = item.Item5,
                        Concept2ContextId = item.Item6
                    })
                    //.GroupBy(item => item.Item2)
                    //.Select(item => new InternalNamespaceDTO { Description = item.Key })
                    //.OrderBy(item => item.Description)
                });
            }

            return notTranslatedConceptToContextGroups;
        }

        #endregion
    }
}
