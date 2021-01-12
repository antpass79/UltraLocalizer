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
    public class JobListService : IJobListService
    {
        private readonly IReadRepository<VJobListConcept> _repository;

        public JobListService(
            IReadRepository<VJobListConcept> repository)
        {
            _repository = repository;
        }

        async public Task<IEnumerable<JobListConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId)
        {
            try
            {
                var items = _repository.Query()
                    .Where(item =>
                        item.JobListLanguageId == languageId)
                    .WhereIf(item =>
                        item.JobListId == jobListId, jobListId != 0)
                    .WhereIf(item =>
                        item.ConceptComponentNamespace == componentNamespace, componentNamespace != SharedConstants.COMPONENT_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.ConceptInternalNamespace == internalNamespace, internalNamespace != SharedConstants.INTERNAL_NAMESPACE_ALL)
                    .ToList()
                    .Select(item => new
                    {
                        ConceptId = item.ConceptId,
                        ComponentNamespace = item.ConceptComponentNamespace,
                        InternalNamespace = item.ConceptInternalNamespace,
                        Concept = item.Concept,
                        ContextName = item.Context,
                        ConceptToContextId = item.ConceptToContextId,
                        StringType = item.StringType,
                        StringValue = item.String,
                        StringId = item.StringId
                    })
                    .ToList();

                var result = items
                .GroupBy(item => item.ConceptId)
                .Select(group => new JobListConcept
                {
                    ComponentNamespace = group.First().ComponentNamespace,
                    InternalNamespace = group.First().InternalNamespace,
                    Id = group.First().ConceptId,
                    Name = group.First().Concept,                    
                    ContextViews = group.Select(item => new JobListContext
                    {
                        StringId = item.StringId.HasValue ? item.StringId.Value : 0,
                        StringType = !string.IsNullOrWhiteSpace(item.StringType) ? Enum.Parse<StringType>(item.StringType) : StringType.Label,
                        StringValue = item.StringValue,
                        Name = item.ContextName,
                        Concept2ContextId = item.ConceptToContextId
                    }).ToList()
                });

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error during JobListService.GetAllAsync({componentNamespace}, {internalNamespace}, {languageId}, {jobListId}), {e.Message}");
            }
        }

        public Task<IEnumerable<JobListConcept>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JobListConcept> GetAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
