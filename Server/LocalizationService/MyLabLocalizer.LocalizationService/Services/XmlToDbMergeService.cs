using MyLabLocalizer.Shared.Services;
using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class XmlToDbMergeService : IXmlToDbMergeService
    {
        private readonly LocalizationContext _context;
        private readonly ILogService _logService;

        public XmlToDbMergeService(LocalizationContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public async Task<IEnumerable<MergiableConcept>> MergeFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var mergiableEntries = PrepareMergiableEntriesForDatabase(keyLocalizationSectionSubsetPairs);

            MergeIntoDatabase(mergiableEntries);

            return await Task.FromResult(mergiableEntries);
        }

        IEnumerable<MergiableConcept> PrepareMergiableEntriesForDatabase(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var xmlEntries = new List<MergiableConcept>();
            var mergiableEntries = new List<MergiableConcept>();
            var contexts = _context.LocContexts.ToList();

            foreach (var pair in keyLocalizationSectionSubsetPairs)
            {
                foreach (var context in pair.Value.Contexts)
                {
                    var contextId = contexts
                                .Where(item => item.ContextName == context)
                                .Select(item => item.Id)
                                .Single();

                    xmlEntries.Add(new MergiableConcept
                    {
                        ComponentNamespace = pair.Value.ComponentNamespace,
                        InternalNamespace = pair.Value.InternalNamespace,
                        Concept = pair.Value.ConceptId,
                        Context = context
                        //DeveloperComment = pair.Value.DeveloperComment//TODO: quando sara' il momento di metterlo su DB
                    });
                }
            }

            var dbEntries = (from concept2Context in _context.LocConcept2Contexts
                            join concept in _context.LocConceptsTables on concept2Context.Idconcept equals concept.Id
                            join context in _context.LocContexts on concept2Context.Idcontext equals context.Id
                            select new MergiableConcept
                            {
                                ConceptId = concept.Id,
                                ComponentNamespace = concept.ComponentNamespace,
                                InternalNamespace = concept.InternalNamespace,
                                Concept = concept.LocalizationId,
                                Context = context.ContextName
                            })
                            .ToList();

            foreach(var entry in xmlEntries)
            {
                var filteredDbEntries = dbEntries
                    .Where(item =>
                        item.ComponentNamespace == entry.ComponentNamespace &&
                        item.InternalNamespace == entry.InternalNamespace &&
                        item.Concept == entry.Concept);

                if (!filteredDbEntries.Any())
                {
                    entry.ActionType = ConceptTuplaActionType.ToInsert;
                    mergiableEntries.Add(entry);
                }
                else if (!filteredDbEntries
                    .Where(item => item.Context == entry.Context)
                    .Any())
                {
                    entry.ActionType = ConceptTuplaActionType.ToUpdate;
                    entry.ConceptId = filteredDbEntries.FirstOrDefault().ConceptId;                  
                    mergiableEntries.Add(entry);
                }
            }

            return mergiableEntries;
        }

        private void MergeIntoDatabase(IEnumerable<MergiableConcept> mergiableEntries)
        {
            var contexts = _context.LocContexts.ToList();
            var mergiableEntryGroups = mergiableEntries.GroupBy(item => new
            {
                item.ComponentNamespace,
                item.InternalNamespace,
                item.Concept,
                //item.DeveloperComment,
                item.ActionType
            });

            foreach (var group in mergiableEntryGroups)
            {
                var concept = new LocConceptsTable
                {
                    ComponentNamespace = group.Key.ComponentNamespace,
                    InternalNamespace = group.Key.InternalNamespace,
                    LocalizationId = group.Key.Concept,
                    Comment = string.Empty,//Il vecchio se non c'e' nulla lo inizializza cosi' //Comment = group.Key.DeveloperComment,
                    Ignore = false   
                    //ConceptId, essendo PK della tabella, viene determinato automaticamente da EF
                };

                if (group.Key.ActionType == ConceptTuplaActionType.ToInsert)
                {
                    _context.LocConceptsTables.Add(concept);

                    _logService.Info($"Concept {group.Key.Concept} has been inserted with Component Namespace {group.Key.ComponentNamespace} and Internal Namespace {group.Key.InternalNamespace}");
                }

                foreach (var element in group)
                {
                    var contextId = contexts
                                    .Where(item => item.ContextName == element.Context)
                                    .Select(item => item.Id)
                                    .Single();
                    var concept2Context = new LocConcept2Context
                    {
                        Idcontext = contextId
                    };

                    if (element.ActionType == ConceptTuplaActionType.ToInsert)
                    {
                        concept.LocConcept2Contexts.Add(concept2Context);
                    }
                    else 
                    {
                        concept2Context.Idconcept = element.ConceptId;
                        _context.LocConcept2Contexts.Add(concept2Context);
                    }

                    _logService.Info($"Concept {element.ConceptId} has been updated with the context {element.Context}");
                }           
            }
        }
    }
}
