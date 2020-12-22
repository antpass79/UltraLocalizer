using Globe.BusinessLogic.Repositories;
using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class NotTranslatedConceptViewService : IAsyncNotTranslatedConceptViewService
    {
        private readonly IReadRepository<VConceptStringToContext> _conceptStringToContextRepository;
        private readonly IReadRepository<VStringsToContext> _stringsToContextRepository;
        private readonly IUltraDBJobGlobal _ultraDBJobGlobal;

        public NotTranslatedConceptViewService(
            IReadRepository<VConceptStringToContext> conceptStringToContextRepository,
            IReadRepository<VStringsToContext> stringsToContextRepository,            
            IUltraDBJobGlobal ultraDBJobGlobal)
        {
            _conceptStringToContextRepository = conceptStringToContextRepository;
            _stringsToContextRepository = stringsToContextRepository;

            _ultraDBJobGlobal = ultraDBJobGlobal;
        }

        async public Task<IEnumerable<NotTranslatedConceptView>> GetAllAsync(NotTranslateConceptViewSearchDTO search)
        {
            try
            {
                var result = search.Language.IsoCoding == Constants.LANGUAGE_EN
                    ?
                    GetConceptToContextsByEnglish(search.ComponentNamespace, search.InternalNamespace)
                    :
                    GetConceptToContextsByLanguage(search.ComponentNamespace, search.InternalNamespace, search.Language);

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during InternalNamespaceGroupService.GetAllAsync({search.ComponentNamespace.Description}, {search.InternalNamespace.Description}, {search.Language.IsoCoding}), {e.Message}");
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
            var internalNamespaceGroups = new List<InternalNamespaceGroup>();

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
                    item.Isocoding == Constants.LANGUAGE_EN &&
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
            //var result = _ultraDBJobGlobal
            //    .FillByComponentNamespace(search.InternalNamespace.Description, search.ComponentNamespace.Description, search.Language.IsoCoding)
            //    .OrderBy(p => p.ComponentNamespace)
            //    .ThenBy(p => p.LocalizationID);

            //return await Task.FromResult(result.Select(concept => new NotTranslatedConceptViewDTO 
            //{ 
            //    Id = concept.ConceptID,
            //    Name = concept.LocalizationID,
            //    ComponentNamespace = new ComponentNamespaceDTO { Description = concept.ComponentNamespace },
            //    InternalNamespace = new InternalNamespaceDTO { Description = concept.InternalNamespace },
            //    ContextViews = concept.Group.Select(context => new ContextViewDTO
            //    {
            //        Name = context.ContextName,
            //        Concept2ContextId = context.IDConcept2Context,
            //        StringId = context.IDString,
            //        StringType = string.IsNullOrWhiteSpace(context.StringType) ? default(StringTypeDTO) : Enum.Parse<StringTypeDTO>(context.StringType),
            //        StringValue = context.DataString
            //    })
            //}));

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
