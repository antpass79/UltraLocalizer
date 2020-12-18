using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class NotTranslatedConceptViewService : IAsyncNotTranslatedConceptViewService
    {
        private readonly IUltraDBJobGlobal _ultraDBJobGlobal;

        public NotTranslatedConceptViewService(IUltraDBJobGlobal ultraDBJobGlobal)
        {
            _ultraDBJobGlobal = ultraDBJobGlobal;
        }

        async public Task<IEnumerable<NotTranslatedConceptView>> GetAllAsync(NotTranslateConceptViewSearchDTO search)
        {
            var result = _ultraDBJobGlobal
                .FillByComponentNamespace(search.InternalNamespace.Description, search.ComponentNamespace.Description, search.Language.IsoCoding)
                .OrderBy(p => p.ComponentNamespace)
                .ThenBy(p => p.LocalizationID);

            return await Task.FromResult(result.Select(concept => new NotTranslatedConceptView 
            { 
                Id = concept.ConceptID,
                Name = concept.LocalizationID,
                ComponentNamespace = new ComponentNamespace { Description = concept.ComponentNamespace },
                InternalNamespace = new InternalNamespace { Description = concept.InternalNamespace },
                ContextViews = concept.Group.Select(context => new JobListContext
                {
                    Name = context.ContextName,
                    Concept2ContextId = context.IDConcept2Context,
                    StringId = context.IDString,
                    StringType = string.IsNullOrWhiteSpace(context.StringType) ? default(StringType) : Enum.Parse<StringType>(context.StringType),
                    StringValue = context.DataString
                })
            }));
        }
    }
}
