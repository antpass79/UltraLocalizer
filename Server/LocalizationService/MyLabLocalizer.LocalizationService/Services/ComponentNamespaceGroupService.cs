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
                var result = language.IsoCoding == SharedConstants.LANGUAGE_EN
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

        async public Task<IEnumerable<ComponentNamespaceGroup>> GetAllAsync()
        {
            try
            {
                var result = GetAllGroups();
                    
                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during ComponentNamespaceGroupService.GetAllAsync(), {e.Message}");
            }
        }

        #endregion

        #region Private Functions

        IEnumerable<ComponentNamespaceGroup> GetGroupsByEnglish()
        {
            var items = _conceptStringToContextRepository
                .Query(item =>
                    item.StringId == null &&
                    item.ComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD)
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
                    item.ComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD)
                .Select(item => item.Id);

            var items = _stringsToContextRepository
                .Query(item =>
                    item.Ignore.HasValue &&
                    !item.Ignore.Value &&
                    item.Isocoding == SharedConstants.LANGUAGE_EN &&
                    item.ComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD &&
                    !subQuery.Contains(item.Id))
                .AsNoTracking()
                .ToList()
                .Select(item =>
                    new Tuple<string, string>(item.ComponentNamespace, item.InternalNamespace));

            return BuildGroups(items);
        }

        IEnumerable<ComponentNamespaceGroup> GetAllGroups()
        {
            var items = _conceptStringToContextRepository
                .Query(item =>
                    item.StringId != null &&
                    item.ComponentNamespace != null &&
                    item.ComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD)
                .AsNoTracking()
                .Select(item =>
                    new Tuple<string, string>(item.ComponentNamespace, item.InternalNamespace))
                .Distinct()
                .ToList();

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
