using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class JobListService : IJobListService
    {
        private readonly LocalizationContext _localizationContext;

        public JobListService(LocalizationContext localizationContext)
        {
            _localizationContext = localizationContext;
        }

        async public Task<IEnumerable<JobListConcept>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobListId)
        {
            try
            {
                var items = _localizationContext.VJobListConcepts
                    .Where(item =>
                        item.JobListLanguageId == languageId)
                    .WhereIf(item =>
                        item.JobListId == jobListId, jobListId != 0) // != all
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
                        StringId = item.StringId,
                        StringInEnglish = item.StringInEnglish
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
                        StringType = !string.IsNullOrWhiteSpace(item.StringType) ? Enum.Parse<StringType>(item.StringType) : StringType.String,
                        StringValue = item.StringValue,
                        StringInEnglish = item.StringInEnglish,
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

        async public Task SaveAsync(NewJobList newJobList)
        {
            var jobList = new LocJobList
            {
                IdisoCoding = newJobList.Language.Id,
                JobName = newJobList.Name,
                UserName = newJobList.User.UserName
            };

            newJobList.Concepts
                .ToList()
                .ForEach(concept =>
                {
                    concept.ContextViews
                        .ToList()
                        .ForEach(context =>
                        {
                            var job2Concept = new LocJob2Concept { Idconcept2Context = context.Concept2ContextId };
                            jobList.LocJob2Concepts.Add(job2Concept);
                            _localizationContext.LocJob2Concepts.Add(job2Concept);
                        });
                });

            _localizationContext.LocJobLists.Add(jobList);

            await _localizationContext.SaveChangesAsync();
        }

        public Task<IEnumerable<JobListConcept>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JobListConcept> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(NewJobList job2Concept)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(NewJobList job2Concept)
        {
            throw new NotImplementedException();
        }
        
        public Task UpdateAsync(NewJobList job2Concept)
        {
            throw new NotImplementedException();
        }
    }
}
