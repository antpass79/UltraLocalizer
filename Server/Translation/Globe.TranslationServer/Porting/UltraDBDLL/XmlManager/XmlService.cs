using Globe.Shared.Services;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Services;
using Globe.TranslationServer.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Porting.UltraDBDLL.XmlManager
{
    public class XmlService : IXmlService
    {
        #region Nested

        private class LocalizationSectionSubset
        {
            public string ComponentNamespace { get; set; }
            public string InternalNamespace { get; set; }
            public string ConceptId { get; set; }
            public string DeveloperComment { get; set; }
            public IEnumerable<string> Contexts { get; set; }
            public string FileName { get; set; }
        }

        #endregion

        #region Data Members

        private readonly LocalizationContext context;
        private readonly ILogService _logService;

        string _xmlDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Constants.XML_FOLDER);
        //lista che contiene tutte le triple component/internal/conceptId della tabella conceptTable (non ci sono triple ripetute)
        IEnumerable<ConceptTupla> _dbTuplasForInsert;
        Dictionary<string, LocalizationSectionSubset> _keyLocalizationSectionSubsetPairs = new Dictionary<string, LocalizationSectionSubset>();
        List<LocalizationResource> _localizationResources = new List<LocalizationResource>();
        List<string> _errors = new List<string>();

        #endregion

        #region Constructors

        public XmlService(LocalizationContext context, ILogService logService)
        {
            this.context = context;
            _logService = logService;
        }

        #endregion

        #region Properties

        public bool ChangesFound { get; private set; }
        public int InsertedCount { get; private set; }
        public int UpdatedCount { get; private set; }

        #endregion

        #region Public Functions

        public string GetOriginalDeveloperString(string componentNamespace,
                                    string internalNamespace,
                                    string conceptId,
                                    string context)
        {
            var originalDeveloperString = (from LocalizationResource in _localizationResources
                                           where LocalizationResource.ComponentNamespace == componentNamespace
                                           from section in LocalizationResource.LocalizationSection
                                           where section.InternalNamespace == internalNamespace
                                           from concept in section.Concept
                                           where concept.Id == conceptId
                                           from @string in concept.String
                                           where @string.Context == context
                                           select @string.TypedValue)
                                           .SingleOrDefault();

            return originalDeveloperString;
        }

        /// <summary>
        /// Carica gli Xml SENZA aggiornare il DB, in modo sequenziale
        /// </summary>
        public void LoadXml()
        {
            _dbTuplasForInsert = context
                .LocConceptsTables
                .Select(concept => new ConceptTupla
                {
                    ComponentNamespace = concept.ComponentNamespace,
                    InternalNamespace = concept.InternalNamespace,
                    ConceptId = concept.LocalizationId
                });

            var files = Directory.GetFiles(_xmlDirectory, "*.definition.xml");
            _logService.Info($"{files.Length} xml files found");

            foreach (string file in files)
            {
                LoadXml(file);
            }
        }

        //Workflow steps
        //- Check conditions
        //- Load xml
        //- Check differences (xml vs DB)
        //- Update/Insert DB (no Delete). With the new DB it will be possible to flag obsolete concepts
        public void FillDB()
        {
            //Set variables before start process
            ChangesFound = false;
            UpdatedCount = 0;
            InsertedCount = 0;

            if (!_keyLocalizationSectionSubsetPairs.Any())
                return;

            WriteDB();
        }

        public async Task<int> UpdateDatabaseAsync()
        {
            var keyLocalizationSectionSubsets = LoadKeyLocalizationSectionSubsetPairs();
            if (!keyLocalizationSectionSubsets.Any())
                throw new Exception("Inconsistent updating process");

            return await UpdateDatabaseFromLocalizationSectionSubsets(keyLocalizationSectionSubsets);
        }

        private Dictionary<string, LocalizationSectionSubset> LoadKeyLocalizationSectionSubsetPairs()
        {
            var files = Directory.GetFiles(_xmlDirectory, "*.definition.xml");
            _logService.Info($"{files.Length} xml files found");
            var keyLocalizationSectionSubsetPairs = new Dictionary<string, LocalizationSectionSubset>();

            foreach (var file in files)
            {
                try
                {
                    _logService.Info($"{file} xml file loaded");

                    var localizationResource = LocalizationResource.Load(file);
                    _localizationResources.Add(localizationResource);
                    var localizationSection = localizationResource.LocalizationSection.Select(section => new
                    {
                        Fields = section.Concept.Select(concept => new LocalizationSectionSubset
                        {
                            ComponentNamespace = localizationResource.ComponentNamespace,
                            InternalNamespace = section.InternalNamespace,
                            ConceptId = concept.Id,
                            DeveloperComment = concept.Comments?.TypedValue,
                            Contexts = concept.String.Select(conceptString => conceptString.Context),
                            FileName = file
                        })
                    });

                    localizationSection
                        .SelectMany(item => item.Fields)
                        .ToList()
                        .ForEach(item =>
                        {
                            var key = BuildKey(item.ComponentNamespace, item.InternalNamespace, item.ConceptId);
                            var comment = item.DeveloperComment;

                            try
                            {
                                keyLocalizationSectionSubsetPairs.Add(key, item);
                            }
                            catch (Exception exception)
                            {
                                _logService.Exception(exception);
                                _errors.Add($"The tupla {key} in the file {file} already exists in a previous processed xml. It will be discarded");
                            }
                        });
                }
                catch (Exception exception)
                {
                    _logService.Exception(exception);
                    _errors.Add(exception.Message);
                }
            }

            return keyLocalizationSectionSubsetPairs;
        }

        private async Task<int> UpdateDatabaseFromLocalizationSectionSubsets(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var (insertableEntries, updatableEntries) = PrepareEntriesForDatabase(keyLocalizationSectionSubsetPairs);

            InsertIntoDatabase(insertableEntries);
            UpdateDatabase(updatableEntries);

            try
            {
                return await context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        (IEnumerable<ConceptTupla>, IEnumerable<ConceptTupla>) PrepareEntriesForDatabase(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var insertableXmlEntries = new List<ConceptTupla>();
            var updatableXmlEntries = new List<ConceptTupla>();

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

                foreach (var context in pair.Value.Contexts)
                {
                    updatableXmlEntries.Add(new ConceptTupla
                    {
                        ComponentNamespace = pair.Value.ComponentNamespace,
                        InternalNamespace = pair.Value.InternalNamespace,
                        ConceptId = pair.Value.ConceptId,
                        ContextId = UltraDBGlobal.UltraDBGlobal.GetContextId(context.Trim()).ToString(),//TODO
                        FileName = pair.Value.FileName
                    });
                }
            }

            var insertedDbEntries = context
                .LocConceptsTables
                .Select(concept => new ConceptTupla
                {
                    ComponentNamespace = concept.ComponentNamespace,
                    InternalNamespace = concept.InternalNamespace,
                    ConceptId = concept.LocalizationId
                });

            var insertableEntries = insertableXmlEntries.Except(insertedDbEntries, new TuplaComparerForInsert());

            var updatedDbEntries = from concept2Context in context.LocConcept2Contexts
                                   join concept in context.LocConceptsTables on concept2Context.Idconcept equals concept.Id
                                   join context in context.LocContexts on concept2Context.Idcontext equals context.Id
                                   select new ConceptTupla
                                   {
                                       Id = concept.Id.ToString(),
                                       ComponentNamespace = concept.ComponentNamespace,
                                       InternalNamespace = concept.InternalNamespace,
                                       ConceptId = concept.LocalizationId,
                                       ContextId = concept2Context.Idcontext.ToString()
                                   };

            var updatableEntries = updatableXmlEntries.Except(updatedDbEntries, new TuplaComparerForUpdate());

            return (insertableEntries.ToList(), updatableEntries.ToList());
        }

        private void InsertIntoDatabase(IEnumerable<ConceptTupla> insertableEntries)
        {
            var contexts = context.LocContexts.ToList();

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

                    context.LocConceptsTables.Add(concept);

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

        private void UpdateDatabase(IEnumerable<ConceptTupla> updatableEntries)
        {
            foreach (var entry in updatableEntries)
            {
                try
                {
                    //TODO Abbiamo id multiple per una stessa combinazione di localizationId/Component/Internal. Idealmente dovremmo avere SingleOrDefault
                    var conceptId = updatableEntries
                        .Where(item => item.ComponentNamespace == entry.ComponentNamespace && item.InternalNamespace == entry.InternalNamespace && item.ConceptId == entry.ConceptId)
                        .FirstOrDefault().Id;
                    //.SingleOrDefault().Id;

                    context.LocConcept2Contexts.Add(new LocConcept2Context
                    {
                        Idconcept = Convert.ToInt32(conceptId),
                        Idcontext = Convert.ToInt32(entry.ContextId.Trim())
                    });

                    _logService.Info($"Concept {entry.ConceptId} has been updated with the context {UltraDBGlobal.UltraDBGlobal.GetContextName(Convert.ToInt32(entry.ContextId.Trim()))}");
                }
                catch (Exception exception)
                {
                    _logService.Exception(exception);

                    var contextName = context.LocContexts
                             .Where(item => item.Id == Convert.ToInt32(entry.ConceptId))
                             .Select(item => item.ContextName)
                             .Single();

                    var error = $"Broken Concept at Component={entry.ComponentNamespace} Internal={entry.InternalNamespace} Concept={entry.ConceptId} Context= {contextName}";

                    _logService.Info(error);
                    _errors.Add(error);
                }
            }
        }

        public string GetSoftwareDeveloperComment(string componentNamespace, string internalNamespace, string conceptId)
        {
            var key = BuildKey(componentNamespace, internalNamespace, conceptId);
            var softwareDeveloperComment = string.Empty;
            try
            {
                softwareDeveloperComment = _keyLocalizationSectionSubsetPairs[key].DeveloperComment;
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                softwareDeveloperComment = "No comment Found";
            }

            return softwareDeveloperComment;
        }

        #endregion

        #region Private Functions

        private string BuildKey(string ComponentNamespace, string InternalNamespace, string ConceptID)
        {
            return string.Format("{0}|{1}|{2}", ComponentNamespace, InternalNamespace, ConceptID);
        }

        private void LoadXml(string file)
        {
            try
            {
                _logService.Info($"{file} xml file loaded");

                var localizationResource = LocalizationResource.Load(file);
                _localizationResources.Add(localizationResource);
                var localizationSection = localizationResource.LocalizationSection.Select(section => new
                {
                    Fields = section.Concept.Select(concept => new LocalizationSectionSubset
                    {
                        ComponentNamespace = localizationResource.ComponentNamespace,
                        InternalNamespace = section.InternalNamespace,
                        ConceptId = concept.Id,
                        DeveloperComment = concept.Comments?.TypedValue,
                        Contexts = concept.String.Select(conceptString => conceptString.Context)
                    })
                });

                localizationSection
                    .SelectMany(item => item.Fields)
                    .ToList()
                    .ForEach(item =>
                    {
                        var key = BuildKey(item.ComponentNamespace, item.InternalNamespace, item.ConceptId);
                        var comment = item.DeveloperComment;

                        try
                        {
                            _keyLocalizationSectionSubsetPairs.Add(key, item);
                        }
                        catch (Exception ex)
                        {
                            _logService.Exception(ex);
                            _errors.Add(string.Format("La tripla {0} nel file {1} risulta già presente in un xml precedentemente processato e pertanto viene scartata", key, file));
                        }
                    });
            }
            catch (Exception ex)
            {
                _logService.Exception(ex);
                _errors.Add(ex.Message);
            }
        }

        private void WriteDB()
        {
            string CNamespace, INamespace, ConceptId;
            try
            {
                var localKeyTuplasForInsert = new List<ConceptTupla>();
                var localKeyTuplasForUpdate = new List<ConceptTupla>();
                IEnumerable<ConceptTupla> dbTuplasForUpdate;
                var concept = new UltraDBConcept.UltraDBConcept(context);
                var conceptToContext = new UltraDBConcept2Context(context);
                foreach (var pair in _keyLocalizationSectionSubsetPairs)
                {
                    //riempo la struttura per l'insert
                    localKeyTuplasForInsert.Add(new ConceptTupla
                    {
                        ComponentNamespace = pair.Value.ComponentNamespace,
                        InternalNamespace = pair.Value.InternalNamespace,
                        ConceptId = pair.Value.ConceptId,
                        Strings = pair.Value.Contexts
                    });

                    foreach (var context in pair.Value.Contexts)
                    {
                        localKeyTuplasForUpdate.Add(new ConceptTupla
                        {
                            ComponentNamespace = pair.Value.ComponentNamespace,
                            InternalNamespace = pair.Value.InternalNamespace,
                            ConceptId = pair.Value.ConceptId,
                            ContextId = UltraDBGlobal.UltraDBGlobal.GetContextId(context.Trim()).ToString()
                        });
                    }
                }

                #region INSERIMENTO NUOVE TUPLE NEL DB
                //eseguo la differenza tra le triple lette nell'xml e quelle presenti nel DB. 
                //l'insieme ottenuto va inserito nel db sia nella tabella Concept che in quella Context2Concept (per la parte relativa alle stringhe)
                var tuplaToInsert = localKeyTuplasForInsert.Except(_dbTuplasForInsert, new TuplaComparerForInsert());
                ChangesFound = tuplaToInsert.Any();
                InsertedCount = tuplaToInsert.Count();

                //inserisco nel DB le nuove tuple sia nella Concept che nella Concept2Context
                foreach (var item in tuplaToInsert)
                {
                    //inserisco la tripla
                    CNamespace = item.ComponentNamespace;
                    INamespace = item.InternalNamespace;
                    ConceptId = item.ConceptId;
                    int conceptId = concept.InsertNewConcept(item.ComponentNamespace, item.InternalNamespace, item.ConceptId, false, null);
                    _logService.Info($"Concept {item.ConceptId} has been inserted with Component Namespace {item.ComponentNamespace} and Internal Namespace {item.InternalNamespace}");
                    //per ogni tripla inserita, inserisco la entry nella concept2context

                    //foreach (string s in item.Strings)
                    //{
                    //    conceptToContext.InsertNewConcept2Context(conceptId, UltraDBGlobal.UltraDBGlobal.GetContextId(s.Trim()));
                    //}
                    foreach (var contextName in item.Strings)
                    {
                        var contextId = context.LocContexts
                             .Where(locContext => locContext.ContextName == contextName)
                             .Select(dbContext => dbContext.Id)
                             .Single();

                        conceptToContext.InsertNewConcept2Context(conceptId, contextId);
                    }
                    //Attenzione classe ConceptTupla, non mi convince. Field ContextId e' sbagliato. Dovrebbe essere una lista, ci deve essere un pastrocchio dentro e ha parecchie referenze
                }
                #endregion

                #region UPDATE TUPLE DB

                dbTuplasForUpdate = concept.GetAllConceptsAndContext();

                var tuplaToUpdate = localKeyTuplasForUpdate.Except(dbTuplasForUpdate, new TuplaComparerForUpdate());
                ChangesFound = ChangesFound || tuplaToUpdate.Any();
                UpdatedCount = tuplaToUpdate.Count();
                //inserisco nella Concept2Context le stringhe nuove associate a Concept già presenti
                foreach (var item in tuplaToUpdate)
                {
                    //ricavo l'ID
                    try
                    {
                        var id = dbTuplasForUpdate.Where(s => s.ComponentNamespace == item.ComponentNamespace && s.InternalNamespace == item.InternalNamespace && s.ConceptId == item.ConceptId).Select(s => s).FirstOrDefault().Id;
                        conceptToContext.InsertNewConcept2Context(Convert.ToInt32(id), Convert.ToInt32(item.ContextId.Trim()));
                        _logService.Info($"Concept {item.ConceptId} has been updated with the context {UltraDBGlobal.UltraDBGlobal.GetContextName(Convert.ToInt32(item.ContextId.Trim()))}");
                    }
                    catch (System.Exception ex)
                    {
                        _logService.Exception(ex);

                        string err = string.Format("Broken Concept at Component={0} Internal={1} Concept={2} Context= {3} ",
                                                    item.ComponentNamespace,
                                                    item.InternalNamespace,
                                                    item.ConceptId,
                                                    UltraDBGlobal.UltraDBGlobal.GetContextName(Convert.ToInt32(item.ContextId.Trim())));
                        _logService.Info(err);
                        _errors.Add(err);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                _logService.Exception(ex);
                _errors.Add(ex.Message);
            }
        }

        #endregion
    }
}
