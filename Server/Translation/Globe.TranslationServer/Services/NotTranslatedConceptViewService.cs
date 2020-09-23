using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class NotTranslatedConceptViewService : IAsyncNotTranslatedConceptViewService
    {
        private readonly UltraDBJobGlobal _ultraDBJobGlobal;

        public NotTranslatedConceptViewService(UltraDBJobGlobal ultraDBJobGlobal)
        {
            _ultraDBJobGlobal = ultraDBJobGlobal;
        }

        async public Task<IEnumerable<NotTranslatedConceptViewDTO>> GetAllAsync(NotTranslateConceptViewSearchDTO search)
        {
            var result = _ultraDBJobGlobal
                .FillByComponentNamespace(search.InternalNamespace.Description, search.ComponentNamespace.Description, search.Language.IsoCoding)
                .OrderBy(p => p.ComponentNamespace)
                .ThenBy(p => p.LocalizationID);

            return await Task.FromResult(result.Select(concept => new NotTranslatedConceptViewDTO 
            { 
                Id = concept.ConceptID,
                Name = concept.LocalizationID,
                ComponentNamespace = new ComponentNamespaceDTO { Description = concept.ComponentNamespace },
                InternalNamespace = new InternalNamespaceDTO { Description = concept.InternalNamespace },
                ContextViews = concept.Group.Select(context => new ContextViewDTO
                {
                    Name = context.ContextName,
                    Concept2ContextId = context.IDConcept2Context,
                    StringId = context.IDString,
                    StringType = string.IsNullOrWhiteSpace(context.StringType) ? default(StringTypeDTO) : Enum.Parse<StringTypeDTO>(context.StringType),
                    StringValue = context.DataString
                })
            }));
        }
    }
}
