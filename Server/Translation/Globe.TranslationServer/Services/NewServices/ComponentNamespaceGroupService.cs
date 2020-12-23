using Globe.BusinessLogic.Repositories;
using Globe.Shared.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class ComponentNamespaceGroupService : IComponentNamespaceGroupService
    {
        #region Data Members

        private readonly IReadRepository<VConceptStringToContext> _conceptStringToContextRepository;
        private readonly IReadRepository<VStringsToContext> _stringsToContextRepository;

        #endregion

        #region Constructors

        public ComponentNamespaceGroupService(
            IReadRepository<VConceptStringToContext> conceptStringToContextRepository,
            IReadRepository<VStringsToContext> stringsToContextRepository)
        {
            _conceptStringToContextRepository = conceptStringToContextRepository;
            _stringsToContextRepository = stringsToContextRepository;
        }

        #endregion

        #region Public Functions

        async public Task<IEnumerable<ComponentNamespaceGroup>> GetAllAsync(Language language)
        {
            try
            {
                var result = language.IsoCoding == Constants.LANGUAGE_EN
                    ?
                    GetGroupsByEnglish()
                    :
                    GetGroupsByLanguage(language);

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during ComponentNamespaceGroupService.GetAllAsync({language.IsoCoding}), {e.Message}");
            }
        }

        #endregion

        #region Private Functions

        IEnumerable<ComponentNamespaceGroup> GetGroupsByEnglish()
        {
            var items = _conceptStringToContextRepository
                .Query(item =>
                    item.StringId == null &&
                    item.ComponentNamespace != Constants.COMPONENT_NAMESPACE_OLD)
                .AsNoTracking()
                .ToList()
                .Select(item =>
                    new Tuple<string, string>(item.ComponentNamespace, item.InternalNamespace));

            return BuildGroups(items);
        }

        IEnumerable<ComponentNamespaceGroup> GetGroupsByLanguage(Language language)
        {
            var componentNamespaceGroups = new List<ComponentNamespaceGroup>();

            var subQuery = _stringsToContextRepository
                .Query(item =>
                    item.Isocoding == language.IsoCoding &&
                    item.ComponentNamespace != Constants.COMPONENT_NAMESPACE_OLD)
                .Select(item => item.Id);

            var items = _stringsToContextRepository
                .Query(item =>
                    item.Ignore.HasValue &&
                    !item.Ignore.Value &&
                    item.Isocoding == Constants.LANGUAGE_EN &&
                    item.ComponentNamespace != Constants.COMPONENT_NAMESPACE_OLD &&
                    !subQuery.Contains(item.Id))
                .AsNoTracking()
                .ToList()
                .Select(item =>
                    new Tuple<string, string>(item.ComponentNamespace, item.InternalNamespace));

            return BuildGroups(items);
        }

        private IEnumerable<ComponentNamespaceGroup> BuildGroups(IEnumerable<Tuple<string, string>> items)
        {
            var componentNamespaceGroups = new List<ComponentNamespaceGroup>();

            var groups = items
                .GroupBy(item => item.Item1)
                .OrderBy(item => item.Key);

            foreach (var group in groups)
            {
                componentNamespaceGroups.Add(new ComponentNamespaceGroup
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

        #endregion
    }
}
