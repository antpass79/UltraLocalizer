using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using Globe.TranslationServer.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class XmlToDBService : IXmlToDBService
    {
        #region Data Members

        private readonly LocalizationContext context;
        private readonly ILogService _logService;
        private readonly IXmlToDbMergeService _xmlToDbMergeService;

        string _xmlDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Constants.XML_FOLDER);
        //TODO: Have to delete after removed GetOriginalDeveloperString and GetSoftwareDeveloperComment
        Dictionary<string, LocalizationSectionSubset> _keyLocalizationSectionSubsetPairs = new Dictionary<string, LocalizationSectionSubset>();
        List<LocalizationResource> _localizationResources = new List<LocalizationResource>();
        List<string> _errors = new List<string>();

        #endregion

        #region Constructors

        public XmlToDBService(
            LocalizationContext context, 
            ILogService logService,
            IXmlToDbMergeService xmlToDbMergeService)
        {
            this.context = context;
            _logService = logService;
            _xmlToDbMergeService = xmlToDbMergeService;

            LoadXml();
        }

        #endregion

        #region Public Functions

        public async Task<XmlServiceStatistics> UpdateDatabaseAsync()
        {
            var keyLocalizationSectionSubsets = LoadKeyLocalizationSectionSubsetPairs();
            if (!keyLocalizationSectionSubsets.Any())
                throw new Exception("Inconsistent updating process");

            return await UpdateDatabaseFromLocalizationSectionSubsets(keyLocalizationSectionSubsets);
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
                //TODO: Da segnalare all'utente che il software development e' vuoto?
                _logService.Exception(e);
                softwareDeveloperComment = "No comment Found";
            }

            return softwareDeveloperComment;
        }

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

        #endregion

        #region Private Functions

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
                            InternalNamespace = String.IsNullOrWhiteSpace(section.InternalNamespace)? null : section.InternalNamespace,
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

        private async Task<XmlServiceStatistics> UpdateDatabaseFromLocalizationSectionSubsets(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs)
        {
            var mergiableEntries = await _xmlToDbMergeService.MergeFilteredEntriesIntoDatabaseAsync(keyLocalizationSectionSubsetPairs);

            var statistics = BuildStatistics(mergiableEntries);

            try 
            {
                await context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            
            return await Task.FromResult(statistics);
        }

        private XmlServiceStatistics BuildStatistics(IEnumerable<MergiableConcept> mergiableEntries)
        {
            var groups = mergiableEntries.GroupBy(item => new
            {
                item.ComponentNamespace,
                item.InternalNamespace,
                item.Concept,
                item.ActionType
            });

            return new XmlServiceStatistics
            {
                InsertedCount = groups.Count(item => item.Key.ActionType == ConceptTuplaActionType.ToInsert),
                UpdatedCount = mergiableEntries.Count(item => item.ActionType == ConceptTuplaActionType.ToUpdate)
            };
        }

        private string BuildKey(string ComponentNamespace, string InternalNamespace, string ConceptID)
        {
            return string.Format("{0}|{1}|{2}", ComponentNamespace, InternalNamespace, ConceptID);
        }

        private void LoadXml()
        {
            var files = Directory.GetFiles(_xmlDirectory, "*.definition.xml");
            _logService.Info($"{files.Length} xml files found");

            foreach (string file in files)
            {
                LoadXml(file);
            }
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

        #endregion
    }
}
