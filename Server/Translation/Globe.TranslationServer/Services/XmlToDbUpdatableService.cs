using Globe.Shared.Services;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class XmlToDbUpdatableService : IXmlToDbUpdatableService
    {
        private readonly LocalizationContext _context;
        private readonly ILogService _logService;

        public XmlToDbUpdatableService(LocalizationContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public async Task<IEnumerable<ConceptTupla>> UpdateFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var updatableEntries = PrepareUpdatableEntriesForDatabase(keyLocalizationSectionSubsetPairs);

            UpdateDatabase(updatableEntries);

            return await Task.FromResult(updatableEntries);
        }

        //Leggo tutti li xml (un domani ci prendiamo sw comment e developer string) -> Tupla con chiavi: Component, internal, concept, context
        //Mi carico in memoria dal DB n Tuple (Tupla con chiavi: Component, internal, concept, context)
        //scorro xml, filtrando il Db
        //Se 
        IEnumerable<ConceptTupla> PrepareUpdatableEntriesForDatabase(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var updatableXmlEntries = new List<ConceptTupla>();
            var contexts = _context.LocContexts.ToList();

            foreach (var pair in keyLocalizationSectionSubsetPairs)
            {
                foreach (var context in pair.Value.Contexts)
                {
                    var contextId = contexts
                                .Where(item => item.ContextName == context)
                                .Select(item => item.Id)
                                .Single();

                    updatableXmlEntries.Add(new ConceptTupla
                    {
                        ComponentNamespace = pair.Value.ComponentNamespace,
                        InternalNamespace = pair.Value.InternalNamespace,
                        ConceptId = pair.Value.ConceptId,
                        ContextId = contextId.ToString(),
                        FileName = pair.Value.FileName
                    });
                }
            }

            var updatedDbEntries = from concept2Context in _context.LocConcept2Contexts
                                   join concept in _context.LocConceptsTables on concept2Context.Idconcept equals concept.Id
                                   join context in _context.LocContexts on concept2Context.Idcontext equals context.Id
                                   select new ConceptTupla
                                   {
                                       Id = concept.Id.ToString(),
                                       ComponentNamespace = concept.ComponentNamespace,
                                       InternalNamespace = concept.InternalNamespace,
                                       ConceptId = concept.LocalizationId,
                                       ContextId = concept2Context.Idcontext.ToString()
                                   };

            var updatableEntries = updatableXmlEntries.Except(updatedDbEntries, new TuplaComparerForUpdate());

            return updatableEntries.ToList();
        }

        private void UpdateDatabase(IEnumerable<ConceptTupla> updatableEntries)
        {
            foreach (var entry in updatableEntries)
            {
                try
                {
                    _context.LocConcept2Contexts.Add(new LocConcept2Context
                    {
                        Idconcept = Convert.ToInt32(entry.Id),
                        Idcontext = Convert.ToInt32(entry.ContextId)
                    });

                    _logService.Info($"Concept {entry.ConceptId} has been updated with the context {UltraDBGlobal.GetContextName(Convert.ToInt32(entry.ContextId.Trim()))}");
                }
                catch (Exception exception)
                {
                    _logService.Exception(exception);

                    var contextName = _context.LocContexts
                             .Where(item => item.Id == Convert.ToInt32(entry.ConceptId))
                             .Select(item => item.ContextName)
                             .Single();

                    var error = $"Broken Concept at Component={entry.ComponentNamespace} Internal={entry.InternalNamespace} Concept={entry.ConceptId} Context= {contextName}";

                    _logService.Info(error);
                    //_errors.Add(error);
                }
            }
        }
    }
}
