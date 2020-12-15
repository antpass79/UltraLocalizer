using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Extensions;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class GroupedStringEntityAdapterService : IAsyncGroupedStringEntityService
    {
        private readonly IReadRepository<VLocalization> _repository;
        private readonly IMapper _mapper;
        private readonly UltraDBEditConcept _ultraDBEditConcept;
        private readonly LocalizationContext _context;

        public GroupedStringEntityAdapterService(
            IReadRepository<VLocalization> repository,
            IMapper mapper,
            UltraDBEditConcept ultraDBEditConcept,
            LocalizationContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _ultraDBEditConcept = ultraDBEditConcept;
            _context = context;
        }

        async public Task<IEnumerable<ConceptViewDTO>> GetAllAsync(string componentNamespace, string internalNamespace, int languageId, int jobItemId)
        {
            try
            {
                var items = _repository.Query()
                    .Where(item =>
                        item.LanguageId == languageId)
                    .WhereIf(item =>
                        item.JobListId == jobItemId, jobItemId != 0)
                    .WhereIf(item =>
                        item.ConceptComponentNamespace == componentNamespace, componentNamespace != Constants.COMPONENT_NAMESPACE_ALL)
                    .WhereIf(item =>
                        item.ConceptInternalNamespace == internalNamespace, internalNamespace != Constants.INTERNAL_NAMESPACE_ALL)
                    .ToList()
                    .Select(item => new
                    {
                        ConceptId = item.ConceptId,
                        ComponentNamespace = item.ConceptComponentNamespace,
                        InternalNamespace = item.ConceptInternalNamespace,
                        Concept = item.Concept,
                        ContextName = item.Context,
                        ContextType = item.StringType,
                        StringValue = item.String,
                        StringId = item.StringId
                    })
                    .ToList();

                var result = items
                .GroupBy(item => new { item.ComponentNamespace, item.InternalNamespace, item.Concept })
                .Select(group => new ConceptViewDTO
                {
                    ComponentNamespace = group.First().ComponentNamespace,
                    InternalNamespace = group.First().InternalNamespace,
                    Id = group.First().ConceptId,
                    Name = group.First().Concept,
                    ContextViews = group.Select(item => new ContextViewDTO
                    {
                        StringId = item.StringId.HasValue ? item.StringId.Value : 0,
                        StringType = !string.IsNullOrWhiteSpace(item.ContextType) ? Enum.Parse<StringTypeDTO>(item.ContextType) : StringTypeDTO.Label,
                        StringValue = item.StringValue,
                        Name = item.ContextName                        
                    }).ToList()
                });

            return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                return new List<ConceptViewDTO>();
            }
        }

        public Task<IEnumerable<ConceptViewDTO>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<ConceptViewDTO> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
