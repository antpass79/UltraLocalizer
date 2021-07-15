using MyLabLocalizer.LocalizationService.Comparers;
using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class ConceptSearchService : IAsyncConceptSearchService
    {
        private readonly LocalizationContext _localizationContext;

        public ConceptSearchService(LocalizationContext localizationContext)
        {
            _localizationContext = localizationContext;
        }

        async public Task<IEnumerable<ConceptSearch>> GetSearchConceptbyISObyContext(string isoCode, string search, string context, bool allData)
        {
            var concepts = await GetSearchConceptbyISO(isoCode, search, allData);
            if (concepts != null && concepts.Count() > 0)
                concepts = concepts.Where(concept => string.Compare(concept.Context.Trim(), context.Trim(), true) == 0);

            return concepts;
        }

        async public Task<IEnumerable<ConceptSearch>> GetSearchConceptbyISObyStringType(string isoCode, string search, string stringType, bool allData)
        {
            var concepts = await GetSearchConceptbyISO(isoCode, search, allData);
            if (concepts != null && concepts.Count() > 0)
                concepts = concepts.Where(concept => string.Compare(concept.Type.Trim(), stringType.Trim(), true) == 0);

            return concepts;
        }

        async public Task<IEnumerable<ConceptSearch>> GetSearchStringtbyISObyContext(string isoCode, string search, string context, bool allData)
        {
            var concepts = await GetSearchStringbyISO(isoCode, search, allData);
            if (concepts != null && concepts.Count() > 0)
                concepts = concepts.Where(concept => string.Compare(concept.Context.Trim(), context.Trim(), true) == 0);

            return concepts;
        }

        async public Task<IEnumerable<ConceptSearch>> GetSearchStringtbyISObyStringType(string isoCode, string search, string stringType, bool allData)
        {
            var concepts = await GetSearchStringbyISO(isoCode, search, allData);
            if (concepts != null && concepts.Count() > 0)
                concepts = concepts.Where(concept => string.Compare(concept.Type.Trim(), stringType.Trim(), true) == 0);

            return concepts;
        }
        //TODO: Refactor multiple joins
        public Task<IEnumerable<ConceptSearch>> GetSearchConceptbyISO(string isoCode, string search, bool allData)
        {
            var concepts = new List<ConceptSearch>();

            try
            {
                var conceptFiltered = _localizationContext.LocConceptsTables.Where(c => EF.Functions.Like(c.LocalizationId.ToUpper(), search.ToUpper().Trim())).Select(c => c);

                var joinc2c = from a in conceptFiltered
                              join b in _localizationContext.LocConcept2Contexts on a.Id equals b.Idconcept
                              select new
                              {
                                  conceptID = a.Id,
                                  c2cID = b.Id,
                                  componentNamespace = a.ComponentNamespace,
                                  internalNamespace = a.InternalNamespace,
                                  localizationID = a.LocalizationId,
                                  comment = a.Comment,
                                  contextID = b.Idcontext
                              };

                var joinConcept = from a in joinc2c
                                  join b in _localizationContext.LocContexts on a.contextID equals b.Id
                                  select new
                                  {
                                      contextName = b.ContextName,
                                      c2cID = a.c2cID,
                                      componentNamespace = a.componentNamespace,
                                      internalNamespace = a.internalNamespace,
                                      localizationID = a.localizationID,
                                      comment = a.comment,
                                  };

                var joinS2C = from a in joinConcept
                              join b in _localizationContext.LocStrings2Contexts on a.c2cID equals b.Idconcept2Context
                              select new
                              {
                                  strID = b.Idstring,
                                  contextName = a.contextName,
                                  componentNamespace = a.componentNamespace,
                                  internalNamespace = a.internalNamespace,
                                  localizationID = a.localizationID,
                                  comment = a.comment
                              };


                var joinString = from a in joinS2C
                                 join b in _localizationContext.LocStrings on a.strID equals b.Id
                                 select new
                                 {
                                     strID = a.strID,
                                     contextName = a.contextName,
                                     componentNamespace = a.componentNamespace,
                                     internalNamespace = a.internalNamespace,
                                     localizationID = a.localizationID,
                                     comment = a.comment,
                                     str = b.String,
                                     IDLan = b.Idlanguage,
                                     IDType = b.Idtype
                                 };

                var joinStrType = from a in joinString
                                  join b in _localizationContext.LocStringTypes on a.IDType equals b.Id
                                  select new
                                  {
                                      strID = a.strID,
                                      contextName = a.contextName,
                                      componentNamespace = a.componentNamespace,
                                      internalNamespace = a.internalNamespace,
                                      localizationID = a.localizationID,
                                      comment = a.comment,
                                      str = a.str,
                                      IDLan = a.IDLan,
                                      strType = b.Type

                                  };


                var joinToLanguage = from a in joinStrType
                                     join b in _localizationContext.LocLanguages on a.IDLan equals b.Id
                                     where b.Isocoding.ToUpper().Equals(isoCode.ToUpper())
                                     select new
                                     {
                                         strID = a.strID,
                                         contextName = a.contextName,
                                         componentNamespace = a.componentNamespace,
                                         internalNamespace = a.internalNamespace,
                                         localizationID = a.localizationID,
                                         comment = a.comment,
                                         str = a.str,
                                         strType = a.strType
                                     };

                int progressiveID = 1;
                foreach (var item in joinToLanguage)
                {
                    ConceptSearch concept = new ConceptSearch();
                    concept.ComponentNamespace = item.componentNamespace;
                    concept.InternalNamespace = item.internalNamespace;
                    concept.Concept = item.localizationID;
                    concept.MasterTranslatorComment = item.comment;
                    concept.StringId = item.strID;
                    concept.Context = item.contextName;
                    concept.Type = item.strType;
                    concept.String = item.str;
                    if (allData || (!concepts.Contains(concept, new ComparerForDBConcept())))
                    {
                        concept.ProgressiveId = progressiveID++;
                        concepts.Add(concept);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                concepts = null;
            }

            return Task.FromResult<IEnumerable<ConceptSearch>>(concepts);
        }
        public Task<IEnumerable<ConceptSearch>> GetSearchStringbyISO(string isoCode, string search, bool allData)
        {
            var concepts = new List<ConceptSearch>();
            try
            {
                var joinStringToStringType = from strings in _localizationContext.LocStrings
                                             join stringType in _localizationContext.LocStringTypes on strings.Idtype equals stringType.Id
                                             where EF.Functions.Like(strings.String.ToUpper(), search.ToUpper())
                                             //where SqlMethods.Like(strings.String.ToUpper(), search.ToUpper())
                                             select new
                                             {
                                                 strID = strings.Id,
                                                 str = strings.String,
                                                 strType = stringType.Type,
                                                 strLan = strings.Idlanguage
                                             };

                var joinToLanguage = from j in joinStringToStringType
                                     join lang in _localizationContext.LocLanguages on j.strLan equals lang.Id
                                     where lang.Isocoding.ToUpper().Equals(isoCode.ToUpper())
                                     select new
                                     {
                                         strID = j.strID,
                                         str = j.str,
                                         strType = j.strType,
                                     };

                var joinToString2Context = from j in joinToLanguage
                                           join s2c in _localizationContext.LocStrings2Contexts on j.strID equals s2c.Idstring
                                           select new
                                           {
                                               strID = j.strID,
                                               str = j.str,
                                               strType = j.strType,
                                               concept2contextID = s2c.Idconcept2Context
                                           };


                var joinConcepts2Context = from s2c in joinToString2Context
                                           join c2c in _localizationContext.LocConcept2Contexts on s2c.concept2contextID equals c2c.Id
                                           select new
                                           {
                                               strID = s2c.strID,
                                               str = s2c.str,
                                               strType = s2c.strType,
                                               contextID = c2c.Idcontext,
                                               conceptID = c2c.Idconcept
                                           };

                var joinContext = from a in joinConcepts2Context
                                  join b in _localizationContext.LocContexts on a.contextID equals b.Id
                                  select new
                                  {
                                      strID = a.strID,
                                      str = a.str,
                                      strType = a.strType,
                                      contextName = b.ContextName,
                                      conceptID = a.conceptID
                                  };

                var joinConcept = from a in joinContext
                                  join b in _localizationContext.LocConceptsTables on a.conceptID equals b.Id
                                  select new
                                  {
                                      strID = a.strID,
                                      str = a.str,
                                      strType = a.strType,
                                      contextName = a.contextName,
                                      conceptID = a.conceptID,
                                      componentNamespace = b.ComponentNamespace,
                                      internalNamespace = b.InternalNamespace,
                                      localizationID = b.LocalizationId,
                                      comment = b.Comment
                                  };

                int progressiveID = 1;
                foreach (var item in joinConcept)
                {
                    ConceptSearch concept = new ConceptSearch();
                    concept.ComponentNamespace = item.componentNamespace;
                    concept.InternalNamespace = item.internalNamespace;
                    concept.Concept = item.localizationID;
                    concept.MasterTranslatorComment = item.comment;
                    concept.StringId = item.strID;
                    concept.Context = item.contextName;
                    concept.Type = item.strType;
                    concept.String = item.str;
                    if (allData || (!concepts.Contains(concept, new ComparerForDBConcept())))
                    {
                        concept.ProgressiveId = progressiveID++;
                        concepts.Add(concept);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                concepts = null;
            }

            return Task.FromResult<IEnumerable<ConceptSearch>>(concepts);
        }
    }
}
