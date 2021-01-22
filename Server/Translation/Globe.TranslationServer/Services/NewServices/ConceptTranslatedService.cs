using Globe.BusinessLogic.Repositories;
using Globe.Shared.DTOs;
using Globe.Shared.Utilities;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
using Globe.TranslationServer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class ConceptTranslatedService : ITranslatedConceptService
    {
        private readonly IReadRepository<VTranslatedConcept> _repository;

        public ConceptTranslatedService(
            IReadRepository<VTranslatedConcept> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<TranslatedConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, string context, string concept, string localizedString)
        {
            try
            {
                var items = _repository.Query()
                    .WhereIf(item =>
                        item.LanguageId == languageId, languageId != 0)
                    .WhereIf(item =>
                        item.ConceptComponentNamespace == componentNamespace, componentNamespace != SharedConstants.COMPONENT_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.ConceptInternalNamespace == internalNamespace, internalNamespace != SharedConstants.INTERNAL_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.Context == context, context != SharedConstants.CONTEXT_ALL)
                    .WhereIf(item =>
                        item.Concept.Contains(concept), !string.IsNullOrEmpty(concept))
                    .WhereIf(item =>
                        item.String.Contains(localizedString), !string.IsNullOrEmpty(localizedString))
                    .ToList()
                    .Select(item => new
                    {                       
                        ComponentNamespace = item.ConceptComponentNamespace,
                        InternalNamespace = item.ConceptInternalNamespace,
                        Concept = item.Concept,
                        ContextName = item.Context,                                             
                        StringType = item.StringType,
                        StringValue = item.String,
                        Language =  item.Language,
                        ConceptToContextId = item.ConceptToContextId
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
                throw new InvalidOperationException($"Error during JobListService.GetAllAsync({componentNamespace}, {internalNamespace}, {languageId}, {context}, {concept}, {localizedString}), {e.Message}");
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
