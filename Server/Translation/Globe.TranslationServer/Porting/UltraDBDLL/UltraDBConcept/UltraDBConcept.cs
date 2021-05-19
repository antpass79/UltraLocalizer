using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept
{
    public class UltraDBConcept
    {
        private readonly LocalizationContext dbContext;

        public UltraDBConcept(LocalizationContext context)
        {
            this.dbContext = context;
        }

        public void UpdateConcept(int ID, bool Ignore, string Comment)
        {
            dbContext.UpdateConcept(ID, Ignore, Comment);
        }

        public DBConcept GetConceptbyID(int ConceptID)
        {
            var item = dbContext.GetDataByID(ConceptID);
            DBConcept concept = new DBConcept();
            concept.ComponentNamespace = item.ComponentNamespace;
            concept.InternalNamespace = item.InternalNamespace;
            concept.LocalizationID = item.LocalizationID;
            concept.IDConcept = ConceptID;
            concept.Comment = item.Comment;
            concept.Ignore = item.Ignore;
            return concept;
        }

        public List<DBConceptSearch> GetSearchConceptbyISO(string ISO, string search, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();

            dbConcept = GetSearchConceptByISOString(ISO, search, allData);
            return dbConcept;
        }

        public List<DBConceptSearch> GetSearchConceptbyISObyContext(string ISO, string search, string context, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();

            List<DBConceptSearch> dbConceptTmp = GetSearchConceptByISOString(ISO, search, allData);
            if (dbConceptTmp != null && dbConceptTmp.Count > 0)
                dbConcept = dbConceptTmp.Where(c => c.Context.Trim().ToUpper() == context.Trim().ToUpper()).Select(c => c).ToList<DBConceptSearch>();

            return dbConcept;
        }

        public List<DBConceptSearch> GetSearchConceptbyISObyStringType(string ISO, string search, string stringType, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();

            List<DBConceptSearch> dbConceptTmp = GetSearchConceptByISOString(ISO, search, allData);

            if (dbConceptTmp != null && dbConceptTmp.Count > 0)
                dbConcept = dbConceptTmp.Where(c => c.Type.Trim().ToUpper() == stringType.Trim().ToUpper()).Select(c => c).ToList<DBConceptSearch>();

            return dbConcept;
        }

        public List<DBConceptSearch> GetSearchStringbyISO(string ISO, string search, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();

            dbConcept = GetSearchDataByISOString(ISO, search, allData);
            return dbConcept;
        }

        public List<DBConceptSearch> GetSearchStringtbyISObyContext(string ISO, string search, string context, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();

            List<DBConceptSearch> dbConceptTmp = GetSearchDataByISOString(ISO, search, allData);

            if (dbConceptTmp != null && dbConceptTmp.Count > 0)
                dbConcept = dbConceptTmp.Where(c => c.Context.Trim().ToUpper() == context.Trim().ToUpper()).Select(c => c).ToList<DBConceptSearch>();

            return dbConcept;
        }

        public List<DBConceptSearch> GetSearchStringtbyISObyStringType(string ISO, string search, string stringType, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();

            List<DBConceptSearch> dbConceptTmp = GetSearchDataByISOString(ISO, search, allData);

            if (dbConceptTmp != null && dbConceptTmp.Count > 0)
                dbConcept = dbConceptTmp.Where(c => c.Type.Trim().ToUpper() == stringType.Trim().ToUpper()).Select(c => c).ToList<DBConceptSearch>();

            return dbConcept;
        }


        #region Private Functions

        private List<DBConceptSearch> GetSearchDataByISOString(string ISO, string search, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();
            try
            {
                var joinStringToStringType = from strings in dbContext.LocStrings
                                             join stringType in dbContext.LocStringTypes on strings.Idtype equals stringType.Id
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
                                     join lang in dbContext.LocLanguages on j.strLan equals lang.Id
                                     where lang.Isocoding.ToUpper().Equals(ISO.ToUpper())
                                     select new
                                     {
                                         strID = j.strID,
                                         str = j.str,
                                         strType = j.strType,
                                     };

                var joinToString2Context = from j in joinToLanguage
                                           join s2c in dbContext.LocStrings2Contexts on j.strID equals s2c.Idstring
                                           select new
                                           {
                                               strID = j.strID,
                                               str = j.str,
                                               strType = j.strType,
                                               concept2contextID = s2c.Idconcept2Context
                                           };


                var joinConcepts2Context = from s2c in joinToString2Context
                                           join c2c in dbContext.LocConcept2Contexts on s2c.concept2contextID equals c2c.Id
                                           select new
                                           {
                                               strID = s2c.strID,
                                               str = s2c.str,
                                               strType = s2c.strType,
                                               contextID = c2c.Idcontext,
                                               conceptID = c2c.Idconcept
                                           };

                var joinContext = from a in joinConcepts2Context
                                  join b in dbContext.LocContexts on a.contextID equals b.Id
                                  select new
                                  {
                                      strID = a.strID,
                                      str = a.str,
                                      strType = a.strType,
                                      contextName = b.ContextName,
                                      conceptID = a.conceptID
                                  };

                var joinConcept = from a in joinContext
                                  join b in dbContext.LocConceptsTables on a.conceptID equals b.Id
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
                    DBConceptSearch concept = new DBConceptSearch();
                    concept.ComponentNamespace = item.componentNamespace;
                    concept.InternalNamespace = item.internalNamespace;
                    concept.LocalizationID = item.localizationID;
                    concept.MTComment = item.comment;
                    concept.IDString = item.strID;
                    concept.Context = item.contextName;
                    concept.Type = item.strType;
                    concept.String = item.str;
                    if (allData || (!dbConcept.Contains(concept, new ComparerForDBConcept())))
                    {
                        concept.progressiveID = progressiveID++;
                        dbConcept.Add(concept);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dbConcept = null;
            }

            return dbConcept;
        }

        private List<DBConceptSearch> GetSearchConceptByISOString(string ISO, string search, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();
            //
            try
            {

                //var conceptFiltered = this.LocConceptsTable.Where(c => SqlMethods.Like(c.LocalizationId.ToUpper(), search.ToUpper().Trim())).Select(c => c);
                var conceptFiltered = dbContext.LocConceptsTables.Where(c => EF.Functions.Like(c.LocalizationId.ToUpper(), search.ToUpper().Trim())).Select(c => c);

                var joinc2c = from a in conceptFiltered
                              join b in dbContext.LocConcept2Contexts on a.Id equals b.Idconcept
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
                                  join b in dbContext.LocContexts on a.contextID equals b.Id
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
                              join b in dbContext.LocStrings2Contexts on a.c2cID equals b.Idconcept2Context
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
                                 join b in dbContext.LocStrings on a.strID equals b.Id
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
                                  join b in dbContext.LocStringTypes on a.IDType equals b.Id
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
                                     join b in dbContext.LocLanguages on a.IDLan equals b.Id
                                     where b.Isocoding.ToUpper().Equals(ISO.ToUpper())
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
                    DBConceptSearch concept = new DBConceptSearch();
                    concept.ComponentNamespace = item.componentNamespace;
                    concept.InternalNamespace = item.internalNamespace;
                    concept.LocalizationID = item.localizationID;
                    concept.MTComment = item.comment;
                    concept.IDString = item.strID;
                    concept.Context = item.contextName;
                    concept.Type = item.strType;
                    concept.String = item.str;
                    if (allData || (!dbConcept.Contains(concept, new ComparerForDBConcept())))
                    {
                        concept.progressiveID = progressiveID++;
                        dbConcept.Add(concept);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dbConcept = null;
            }

            return dbConcept;
        }

        #endregion
    }
}
