using DocumentFormat.OpenXml.InkML;
using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class JobListService : IAsyncJobListService
    {
        const string ISO_CODING_EN = "en";
        const bool IS_MASTER = true;

        private readonly LocalizationContext _localizationContext;
        private readonly UltraDBStrings _ultraDBStrings;
        private readonly UltraDBStrings2Context _ultraDBStrings2Context;
        private readonly UltraDBConcept _ultraDBConcept;

        public JobListService(
            LocalizationContext localizationContext,
            UltraDBStrings ultraDBStrings,
            UltraDBStrings2Context ultraDBStrings2Context,
            UltraDBConcept ultraDBConcept)
        {
            _localizationContext = localizationContext;
            _ultraDBStrings = ultraDBStrings;
            _ultraDBStrings2Context = ultraDBStrings2Context;
            _ultraDBConcept = ultraDBConcept;
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
                            jobList.LocJob2Concept.Add(job2Concept);
                            _localizationContext.LocJob2Concept.Add(job2Concept);
                        });
                });

            _localizationContext.LocJobList.Add(jobList);

            await _localizationContext.SaveChangesAsync();
        }
    }
}
