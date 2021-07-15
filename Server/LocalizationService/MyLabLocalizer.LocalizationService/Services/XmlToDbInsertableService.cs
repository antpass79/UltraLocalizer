using MyLabLocalizer.Shared.Services;
using MyLabLocalizer.LocalizationService.Comparers;
using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class XmlToDbInsertableService : IXmlToDbInsertableService
    {
        private readonly LocalizationContext _context;
        private readonly ILogService _logService;

        public XmlToDbInsertableService(LocalizationContext context, ILogService logService )
        {
            _context = context;
            _logService = logService;
        }

        public async Task<IEnumerable<ConceptTupla>> InsertFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var insertableEntries = PrepareInsertableEntriesForDatabase(keyLocalizationSectionSubsetPairs);

            InsertIntoDatabase(insertableEntries);

            return await Task.FromResult(insertableEntries);
        }

        IEnumerable<ConceptTupla> PrepareInsertableEntriesForDatabase(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var insertableXmlEntries = new List<ConceptTupla>();

            foreach (var pair in keyLocalizationSectionSubsetPairs)
            {
                insertableXmlEntries.Add(new ConceptTupla
                {
                    ComponentNamespace = pair.Value.ComponentNamespace,
                    InternalNamespace = pair.Value.InternalNamespace,
                    ConceptId = pair.Value.ConceptId,
                    Strings = pair.Value.Contexts,
                    FileName = pair.Value.FileName
                });
            }

            var insertedDbEntries = _context
                .LocConceptsTables
                .Select(concept => new ConceptTupla
                {
                    ComponentNamespace = concept.ComponentNamespace,
                    InternalNamespace = concept.InternalNamespace,
                    ConceptId = concept.LocalizationId
                });

            var insertableEntries = insertableXmlEntries.Except(insertedDbEntries, new TuplaComparerForInsert());

            return insertableEntries.ToList();
        }

        private void InsertIntoDatabase(IEnumerable<ConceptTupla> insertableEntries)
        {
            var contexts = _context.LocContexts.ToList();

            foreach (var entry in insertableEntries)
            {
                try
                {
                    var concept = new LocConceptsTable
                    {
                        ComponentNamespace = entry.ComponentNamespace,
                        InternalNamespace = entry.InternalNamespace,
                        LocalizationId = entry.ConceptId,
                        Ignore = false,
                        Comment = null
                    };

                    _context.LocConceptsTables.Add(concept);

                    _logService.Info($"Concept {entry.ConceptId} has been inserted with Component Namespace {entry.ComponentNamespace} and Internal Namespace {entry.InternalNamespace}");

                    foreach (var contextName in entry.Strings)
                    {
                        var contextId = contexts
                                .Where(item => item.ContextName == contextName)
                                .Select(item => item.Id)
                                .Single();

                        var concept2Context = new LocConcept2Context
                        {
                            Idcontext = contextId
                        };

                        concept.LocConcept2Contexts.Add(concept2Context);
                    }
                }
                catch (Exception exception)
                {
                    _logService.Exception(exception);
                }
            }
        }
    }
}
