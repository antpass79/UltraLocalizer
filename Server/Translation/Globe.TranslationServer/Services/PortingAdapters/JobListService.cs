using Globe.Shared.DTOs;
using Globe.TranslationServer.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class JobListService : IAsyncJobListService
    {
        private readonly LocalizationContext _localizationContext;
        public JobListService(
            LocalizationContext localizationContext)
        {
            _localizationContext = localizationContext;
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
    }
}
