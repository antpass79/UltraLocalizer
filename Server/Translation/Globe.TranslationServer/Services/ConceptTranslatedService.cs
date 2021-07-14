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
    public class ConceptTranslatedService : ITranslatedConceptService
    {
        private readonly IReadRepository<VTranslatedConcept> _repository;

        public ConceptTranslatedService(
            IReadRepository<VTranslatedConcept> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<TranslatedConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, string context, string concept, string localizedString, int stringId)
        {
            try
            {
                var conceptTrimmed = concept.Trim('%');
                var localizedStringTrimmed = localizedString.Trim('%');

                var items = _repository.Query()
                    .WhereIf(item =>
                        item.LanguageId == languageId, languageId != SharedConstants.ID_TO_IGNORE)
                    .WhereIf(item =>
                        item.StringId == stringId, stringId != SharedConstants.ID_TO_IGNORE)
                    .WhereIf(item =>
                        item.ConceptComponentNamespace == componentNamespace, componentNamespace != SharedConstants.COMPONENT_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.ConceptInternalNamespace == internalNamespace, internalNamespace != SharedConstants.INTERNAL_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.Context == context, context != SharedConstants.CONTEXT_ALL)
                    .WhereIf(item =>
                        item.Concept == concept, (!string.IsNullOrEmpty(concept) && !concept.Contains('%')))
                    .WhereIf(item =>
                        item.Concept.Contains(conceptTrimmed), (!string.IsNullOrEmpty(concept) && concept.StartsWith('%') && concept.EndsWith('%')))
                    .WhereIf(item =>
                        item.Concept.EndsWith(conceptTrimmed), (!string.IsNullOrEmpty(concept) && concept.StartsWith('%') && !concept.EndsWith('%')))
                    .WhereIf(item =>
                        item.Concept.StartsWith(conceptTrimmed), (!string.IsNullOrEmpty(concept) && !concept.StartsWith('%') && concept.EndsWith('%')))
                    .WhereIf(item =>
                        item.String == localizedString, (!string.IsNullOrEmpty(localizedString) && !localizedString.Contains('%')))
                    .WhereIf(item =>
                        item.String.Contains(localizedStringTrimmed), (!string.IsNullOrEmpty(localizedString) && localizedString.StartsWith('%') && localizedString.EndsWith('%')))
                    .WhereIf(item =>
                        item.String.EndsWith(localizedStringTrimmed), (!string.IsNullOrEmpty(localizedString) && localizedString.StartsWith('%') && !localizedString.EndsWith('%')))
                    .WhereIf(item =>
                        item.String.StartsWith(localizedStringTrimmed), (!string.IsNullOrEmpty(localizedString) && !localizedString.StartsWith('%') && localizedString.EndsWith('%')))
                    .ToList()
                    .Select(item => new
                    {
                        ComponentNamespace = item.ConceptComponentNamespace,
                        InternalNamespace = item.ConceptInternalNamespace,
                        Concept = item.Concept,
                        ContextName = item.Context,
                        StringType = item.StringType,
                        StringValue = item.String,
                        Language = item.Language,
                        ConceptToContextId = item.ConceptToContextId,
                        StringId = item.StringId
                    })
                    .ToList();

                var result = items
                .GroupBy(item => item.ConceptToContextId)
                .Select(group => new TranslatedConcept
                {
                    ComponentNamespace = group.First().ComponentNamespace,
                    InternalNamespace = group.First().InternalNamespace,
                    Concept = group.First().Concept,
                    TranslatedConceptDetails = group.Select(item => new TranslatedConceptDetail
                    {
                        StringType = !string.IsNullOrWhiteSpace(item.StringType) ? Enum.Parse<StringType>(item.StringType) : StringType.Label,
                        LocalizedString = item.StringValue,
                        ContextName = item.ContextName,
                        Language = item.Language
                    }).ToList()
                });

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during ConceptTranslatedService.GetAllAsync({componentNamespace}, {internalNamespace}, {languageId}, {context}, {concept}, {localizedString}), {e.Message}");
            }
        }

        async public Task<IEnumerable<LocalizeString>> GetAllStringsAsync(string componentNamespace, string internalNamespace, int languageId, string context, string concept, string localizedString, int stringId)
        {
            try
            {
                var conceptTrimmed = concept.Trim('%');
                var localizedStringTrimmed = localizedString.Trim('%');

                var items = _repository.Query()
                    .WhereIf(item =>
                        item.LanguageId == languageId, languageId != SharedConstants.ID_TO_IGNORE)
                    .WhereIf(item =>
                        item.StringId == stringId, stringId != SharedConstants.ID_TO_IGNORE)
                    .WhereIf(item =>
                        item.ConceptComponentNamespace == componentNamespace, componentNamespace != SharedConstants.COMPONENT_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.ConceptInternalNamespace == internalNamespace, internalNamespace != SharedConstants.INTERNAL_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.Context == context, context != SharedConstants.CONTEXT_ALL)
                    .WhereIf(item =>
                        item.Concept == concept, (!string.IsNullOrEmpty(concept) && !concept.Contains('%')))
                    .WhereIf(item =>
                        item.Concept.Contains(conceptTrimmed), (!string.IsNullOrEmpty(concept) && concept.StartsWith('%') && concept.EndsWith('%')))
                    .WhereIf(item =>
                        item.Concept.EndsWith(conceptTrimmed), (!string.IsNullOrEmpty(concept) && concept.StartsWith('%') && !concept.EndsWith('%')))
                    .WhereIf(item =>
                        item.Concept.StartsWith(conceptTrimmed), (!string.IsNullOrEmpty(concept) && !concept.StartsWith('%') && concept.EndsWith('%')))
                    .WhereIf(item =>
                        item.String == localizedString, (!string.IsNullOrEmpty(localizedString) && !localizedString.Contains('%')))
                    .WhereIf(item =>
                        item.String.Contains(localizedStringTrimmed), (!string.IsNullOrEmpty(localizedString) && localizedString.StartsWith('%') && localizedString.EndsWith('%')))
                    .WhereIf(item =>
                        item.String.EndsWith(localizedStringTrimmed), (!string.IsNullOrEmpty(localizedString) && localizedString.StartsWith('%') && !localizedString.EndsWith('%')))
                    .WhereIf(item =>
                        item.String.StartsWith(localizedStringTrimmed), (!string.IsNullOrEmpty(localizedString) && !localizedString.StartsWith('%') && localizedString.EndsWith('%')))
                    .ToList()
                    .Select(item => new
                    {
                        ComponentNamespace = item.ConceptComponentNamespace,
                        InternalNamespace = item.ConceptInternalNamespace,
                        Concept = item.Concept,
                        ConceptId = item.ConceptId,
                        ContextName = item.Context,
                        StringType = item.StringType,
                        StringTypeId = item.StringTypeId,
                        StringValue = item.String,
                        Language = item.Language,
                        LanguageId = item.LanguageId,
                        ConceptToContextId = item.ConceptToContextId,
                        StringId = item.StringId,
                        Ignore =  item.ConceptIgnore
                    })
                    .ToList();

                var result = items
                .GroupBy(item => item.StringId)
                .Select(group => new LocalizeString
                {
                    Language = group.First().Language,
                    LanguageId = group.First().LanguageId,
                    StringId = group.First().StringId,
                    Value = group.First().StringValue,
                    StringType = group.First().StringType,//Puo' essere vuoto. e' un problema?
                    StringTypeId = group.First().StringTypeId,

                    LocalizeStringDetails = group.Select(item => new LocalizeStringDetail
                    {
                        ContextName = item.ContextName,    
                        Concept = item.Concept,
                        ConceptId = item.ConceptId,
                        ComponentNameSpace = item.ComponentNamespace,
                        InternalNameSpace = item.InternalNamespace,
                        Concept2ContextId = item.ConceptToContextId,
                        Ignore = item.Ignore.HasValue && item.Ignore.Value

                    }).ToList()
                });

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during ConceptTranslatedService.GetAllAsync({componentNamespace}, {internalNamespace}, {languageId}, {context}, {concept}, {localizedString}), {e.Message}");
            }
        }

        Task<IEnumerable<TranslatedConcept>> IAsyncReadService<TranslatedConcept, int>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<TranslatedConcept> IAsyncReadService<TranslatedConcept, int>.GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
