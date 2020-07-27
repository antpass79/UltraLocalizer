using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
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

        public int InsertNewConcept(string ComponentNamespace, string InternalNamespace, string LocalizationID, bool Ignore, string Comment)
        {
            return dbContext.InsertNewConcept(ComponentNamespace, InternalNamespace, LocalizationID, Ignore, Comment);
        }

        public void UpdateConcept(int ID, bool Ignore, string Comment)
        {
            dbContext.UpdateConcept(ID, Ignore, Comment);
        }

        public void CleanOrphanedConcepts()
        {
            dbContext.CleanOrphanedConcepts();
        }

        public IEnumerable<ConceptTupla> GetAllConcepts()
        {
            var result = dbContext.GetData();
            var tripla = result.Select(s => new ConceptTupla { ComponentNamespace = s.ComponentNamespace, InternalNamespace = s.InternalNamespace, ConceptId = s.LocalizationID });
            return tripla;
        }

        public IEnumerable<ConceptTupla> GetFullConcepts()
        {
            var result = dbContext.GetData();
            var tripla = result.Select(s => new ConceptTupla { Id = s.ID.ToString(), ComponentNamespace = s.ComponentNamespace, InternalNamespace = s.InternalNamespace, ConceptId = s.LocalizationID });
            return tripla;
        }

        public List<int> GetAllConcepts2ContextIDFromConcept(int conceptTableID)
        {
            var result = dbContext.GetConcept2ContextIDsByConceptTableID(conceptTableID);

            List<int> ret = new List<int>();

            foreach (var item in result)
            {
                ret.Add(item.IDConcept2Context);
            }

            return ret;
        }

        public IEnumerable<ConceptTupla> GetAllConceptsAndContext()
        {
            var result = dbContext.GetConceptAndContextData();
            var tripla = result.Select(s => new ConceptTupla { Id = s.Id.ToString(), ComponentNamespace = s.ComponentNamespace, InternalNamespace = s.InternalNamespace, ConceptId = s.LocalizationID, ContextId = s.IDContext.ToString() });
            return tripla;
        }

        public List<int> GetDataC2C()
        {
            List<int> retList = new List<int>();
            var result = dbContext.GetAllC2CData();
            foreach (var item in result)
            {
                retList.Add(item);
            }
            return retList;
        }

        public IEnumerable<ConceptTupla> GetFullConceptsAndContext()
        {
            var result = dbContext.GetConceptAndContextData();
            var tripla = result.Select(s => new ConceptTupla { IdConcept2Context = s.IDConcept2Context.ToString(), Id = s.Id.ToString(), ComponentNamespace = s.ComponentNamespace, InternalNamespace = s.InternalNamespace, ConceptId = s.LocalizationID, ContextId = s.IDContext.ToString() });
            return tripla;
        }

        public List<DBConcept> GetConceptbyConcept2Context(List<int> IDConcept2ContextList)
        {
            List<DBConcept> dbConceptList = new List<DBConcept>();
            foreach (int IDConcept2Context in IDConcept2ContextList)
            {
                IEnumerable<Concept2ContextID> t = dbContext.GetDataByConcept2Context(IDConcept2Context);
                if (t.Count() > 0)
                {
                    DBConcept db = new DBConcept();
                    db.ComponentNamespace = t.ElementAt(0).ComponentNamespace;
                    db.IDConcept2Context = IDConcept2Context;
                    db.InternalNamespace = t.ElementAt(0).InternalNamespace;
                    db.LocalizationID = t.ElementAt(0).LocalizationID;
                    db.Mark2Delete = true;
                    db.Context = t.ElementAt(0).ContextName;
                    dbConceptList.Add(db);
                }
            }
            return dbConceptList;
        }

        public List<DBInternalNameSpace> GetAllInternalNamebyComponent(string Component)
        {
            var internalComponents = dbContext.GetInternalByComponent(Component);
            List<DBInternalNameSpace> retList = new List<DBInternalNameSpace>();
            DBInternalNameSpace n = new DBInternalNameSpace();
            n.InternalNamespace = "all";
            retList.Add(n);
            foreach (var internalComponent in internalComponents)
            {
                DBInternalNameSpace n1 = new DBInternalNameSpace();
                n1.InternalNamespace = string.IsNullOrWhiteSpace(internalComponent.InternalNamespace) ? "null" : internalComponent.InternalNamespace;
                retList.Add(n1);
            }
            return retList;
        }

        public List<DBConcept> FillEraseList(List<int> listConceptID)
        {
            List<DBConcept> dbConcept = new List<DBConcept>();
            foreach (int id in listConceptID)
            {
                var item = dbContext.GetDataByID(id);
                DBConcept concept = new DBConcept();
                concept.ComponentNamespace = item.ComponentNamespace;
                concept.InternalNamespace = item.InternalNamespace;
                concept.LocalizationID = item.LocalizationID;
                concept.IDConcept = id;
                concept.Mark2Delete = true;
                dbConcept.Add(concept);
            }
            return dbConcept;
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

        public List<DBComponent> GetAllComponent()
        {
            List<DBComponent> retList = new List<DBComponent>();
            var componentNames = dbContext.GetAllComponentName();
            DBComponent s = new DBComponent();
            s.ComponentNamespace = "all";
            retList.Add(s);
            foreach (var componentName in componentNames)
            {
                DBComponent s1 = new DBComponent();
                s1.ComponentNamespace = componentName.ComponentNamespace;
                retList.Add(s1);
            }
            return retList;
        }

        public List<DBConceptSearch> GetSearchStringByISOComponentInternalConceptStringType(string ISO, string Component, string Internal, string Concept, string StrType)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();
            //
            try
            {
                var conceptFiltered = dbContext.LocConceptsTable.Where(c => c.LocalizationId == Concept && c.ComponentNamespace == Component && c.InternalNamespace == Internal).Select(c => c);
                var joinc2c = from a in conceptFiltered
                              join b in dbContext.LocConcept2Context on a.Id equals b.Idconcept
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
                              join b in dbContext.LocStrings2Context on a.c2cID equals b.Idconcept2Context
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
                                  where b.Type == StrType
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
                    if (!dbConcept.Contains(concept, new ComparerForDBConcept()))
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

        public List<DBConceptSearch> GetSearchStringByIDString(int stringID, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();

            dbConcept = GetSearchDataByIDString(stringID, allData);
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

        private List<DBConceptSearch> GetSearchDataByIDString(int stringID, bool allData)
        {
            List<DBConceptSearch> dbConcept = new List<DBConceptSearch>();
            try
            {
                var joinStringToStringType = from strings in dbContext.LocStrings
                                             join stringType in dbContext.LocStringTypes on strings.Idtype equals stringType.Id
                                             where strings.Id == stringID
                                             select new
                                             {
                                                 strID = strings.Id,
                                                 str = strings.String,
                                                 strType = stringType.Type,
                                                 strLan = strings.Idlanguage
                                             };

                var joinToLanguage = from j in joinStringToStringType
                                     join lang in dbContext.LocLanguages on j.strLan equals lang.Id
                                     //where lang.ISOCoding.ToUpper().Equals(ISO.ToUpper())
                                     select new
                                     {
                                         strID = j.strID,
                                         str = j.str,
                                         strType = j.strType,
                                     };

                var joinToString2Context = from j in joinToLanguage
                                           join s2c in dbContext.LocStrings2Context on j.strID equals s2c.Idstring
                                           select new
                                           {
                                               strID = j.strID,
                                               str = j.str,
                                               strType = j.strType,
                                               concept2contextID = s2c.Idconcept2Context
                                           };


                var joinConcepts2Context = from s2c in joinToString2Context
                                           join c2c in dbContext.LocConcept2Context on s2c.concept2contextID equals c2c.Id
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
                                  join b in dbContext.LocConceptsTable on a.conceptID equals b.Id
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
                                           join s2c in dbContext.LocStrings2Context on j.strID equals s2c.Idstring
                                           select new
                                           {
                                               strID = j.strID,
                                               str = j.str,
                                               strType = j.strType,
                                               concept2contextID = s2c.Idconcept2Context
                                           };


                var joinConcepts2Context = from s2c in joinToString2Context
                                           join c2c in dbContext.LocConcept2Context on s2c.concept2contextID equals c2c.Id
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
                                  join b in dbContext.LocConceptsTable on a.conceptID equals b.Id
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
                var conceptFiltered = dbContext.LocConceptsTable.Where(c => EF.Functions.Like(c.LocalizationId.ToUpper(), search.ToUpper().Trim())).Select(c => c);

                var joinc2c = from a in conceptFiltered
                              join b in dbContext.LocConcept2Context on a.Id equals b.Idconcept
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
                              join b in dbContext.LocStrings2Context on a.c2cID equals b.Idconcept2Context
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
