using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal
{
    public class UltraDBEditConcept
    {
        public static int TotalConcepts = 0;
        private readonly LocalizationContext dbContext;

        public UltraDBEditConcept(LocalizationContext context)
        {
            this.dbContext = context;
        }

        #region Public Functions

        public List<GroupedStringEntity> GetGroupledDataBy(string ComponentName, string InternalNamespace, string iso, int idJobList)
        {
            return GetGroupledDataByInt(ComponentName, InternalNamespace, (int)UltraDBStrings.UltraDBStrings.ParseFromString(iso), idJobList, iso);
        }

        public List<GroupedStringEntity> GetGroupledDataByInt(string ComponentName, string InternalNamespace, int iso, int idJobList, string strIso)
        {
            List<GroupedStringEntity> retList = new List<GroupedStringEntity>();
            if (ComponentName == "all")
            {
                // loop su tutti i componenti
                UltraDBConcept.UltraDBConcept ultraDBConcept = new UltraDBConcept.UltraDBConcept(dbContext);
                List<DBComponent> comList = ultraDBConcept.GetAllComponent();
                foreach (DBComponent db in comList)
                {
                    if (db.ComponentNamespace == "OLD") continue;
                    if (db.ComponentNamespace == "all") continue;
                    FillAllInternal(db.ComponentNamespace, iso, idJobList, strIso, retList);
                }
            }
            else
            {
                if (InternalNamespace == "all")
                {
                    FillAllInternal(ComponentName, iso, idJobList, strIso, retList);
                }
                else
                    FillByComponentNamespace(InternalNamespace, ComponentName, iso, idJobList, strIso, ref retList);
            }
            TotalConcepts = retList.Count;
            return retList;
        }

        public void CleanupDuplicated()
        {
            var duplicates = dbContext.LocStrings2Context.GroupBy(i => new { i.Idconcept2Context, i.IdstringNavigation.Idlanguage, i.IdstringNavigation.Idtype })
                              .Where(g => g.Count() > 1)
                              .Select(g => g.Key);

            foreach (var x in duplicates)
            {
                List<LocStrings2Context> zList = (from c in dbContext.LocStrings2Context
                                                   where c.Idconcept2Context == x.Idconcept2Context && c.IdstringNavigation.Idlanguage == x.Idlanguage
                                                   select c).ToList();
                if (zList.Count > 1)
                {
                    dbContext.LocStrings2Context.RemoveRange(zList);
                }

            }

            dbContext.SaveChanges();

            var usedStrings = (from x in dbContext.LocStrings2Context
                               select x.Idstring).Distinct();
            var orphanedStrings = (from c in dbContext.LocStrings
                                   where !usedStrings.Contains(c.Id)
                                   select c).ToList();

            dbContext.LocStrings.RemoveRange(orphanedStrings);
            dbContext.SaveChanges();
        }

        public List<JobGroupedStringEntity> FillByJob(int IDJob, int IDisoJob)
        {
            List<JobGroupedStringEntity> retList = new List<JobGroupedStringEntity>();
            IEnumerable<DataTableEditConcept> dt = dbContext.GetDataByJob(IDJob);
            var Concepts = (from p in dt
                            group new
                            {
                                LocalizationID = p.LocalizationID,
                                ComponentNamespace = p.ComponentNamespace,
                                InternalNamespace = p.InternalNamespace,
                                IDConcept2Context = p.ID,
                                ContextName = p.ContextName,
                                IDConcept = p.IDConcept,
                                MTComment = p.Comment,
                                EngString = (from c in dbContext.LocStrings2Context
                                             where c.IdstringNavigation.Idlanguage == 1 && c.Idconcept2Context == p.ID
                                             select c.IdstringNavigation.String).FirstOrDefault(),
                                IDStringType = (from c in dbContext.LocStrings2Context
                                                where c.IdstringNavigation.Idlanguage == 1 && c.Idconcept2Context == p.ID
                                                select c.IdstringNavigation.Idtype).FirstOrDefault(),
                                edtString = (from c in dbContext.LocStrings2Context
                                             where c.IdstringNavigation.Idlanguage == IDisoJob && c.Idconcept2Context == p.ID
                                             select c.IdstringNavigation.String).FirstOrDefault(),
                                IDedtString = (from c in dbContext.LocStrings2Context
                                               where c.IdstringNavigation.Idlanguage == IDisoJob && c.Idconcept2Context == p.ID
                                               select c.IdstringNavigation.Id).FirstOrDefault(),

                            }
                           by new { p.ComponentNamespace, p.InternalNamespace, p.LocalizationID } into MyGroup
                            select new JobGroupedStringEntity
                            {
                                ComponentNamespace = MyGroup.Key.ComponentNamespace,
                                InternalNamespace = MyGroup.Key.InternalNamespace,
                                LocalizationID = MyGroup.Key.LocalizationID,
                                ConceptID = (from c in MyGroup
                                             select c.IDConcept).FirstOrDefault(),
                                MTString = (from c in MyGroup
                                            select c.MTComment).FirstOrDefault(),
                                Group = (from c in MyGroup
                                         select new JobStringEntity
                                         {
                                             ContextName = c.ContextName,
                                             IDConcept = c.IDConcept,
                                             IDConcept2Context = c.IDConcept2Context,
                                             DataString = c.edtString,
                                             IDString = c.IDedtString,
                                             StringENG = c.EngString,
                                             IDStringType = c.IDStringType,
                                             IDiso = IDisoJob
                                         }).ToList()
                            }).OrderBy(z => z.ComponentNamespace).ThenBy(z => z.InternalNamespace).ThenBy(z => z.LocalizationID);

            retList = Concepts.ToList();
            return retList;
        }

        public List<MultiLangString> MLGetBy(string ComponentName, string InternalNamespace)
        {
            return MLGetByInt(ComponentName, InternalNamespace);
        }

        public List<MultiLangString> MLGetByInt(string ComponentName, string InternalNamespace)
        {
            List<MultiLangString> retList = new List<MultiLangString>();
            if (ComponentName == "all")
            {
                // loop su tutti i componenti
                UltraDBConcept.UltraDBConcept inComponent = new UltraDBConcept.UltraDBConcept(dbContext);
                List<DBComponent> comList = inComponent.GetAllComponent();
                foreach (DBComponent db in comList)
                {
                    if (db.ComponentNamespace == "OLD") continue;
                    if (db.ComponentNamespace == "all") continue;
                    MLFillAllInternal(db.ComponentNamespace, ref retList);
                }
            }
            else
            {
                if (InternalNamespace == "all")
                {
                    MLFillAllInternal(ComponentName, ref retList);
                }
                else
                    MLFillByComponentNamespace(InternalNamespace, ComponentName, ref retList);
            }
            return retList;
        }

        #endregion

        #region Private Functions

        private void FillAllInternal(string ComponentName, int iso, int idJobList, string strIso, List<GroupedStringEntity> retList)
        {
            // loop su tutti gli internal
            UltraDBConcept.UltraDBConcept inConcept = new UltraDBConcept.UltraDBConcept(dbContext);
            List<DBInternalNameSpace> inList = inConcept.GetAllInternalNamebyComponent(ComponentName);
            foreach (DBInternalNameSpace db in inList)
            {
                if (db.InternalNamespace == "all") continue;
                FillByComponentNamespace(db.InternalNamespace, ComponentName, iso, idJobList, strIso, ref retList);
            }
        }

        private void FillEngData(string InternalNamespace, string ComponentName, int idJobList, int iso, ref List<GroupedStringEntity> retList)
        {
            IEnumerable<GroupledData> dt;
            if (InternalNamespace == null || InternalNamespace == "" || InternalNamespace == "null")
                InternalNamespace = null;
            dt = dbContext.GetEngDatabyComponentInternal(idJobList, ComponentName, InternalNamespace);

            if (iso == 1)
            {
                var Concepts = from p in dt
                               group new StringEntity
                               {
                                   IDConcept = p.ID,
                                   IDString = p.IDString,
                                   IDConcept2Context = p.IDConcept2Context,
                                   ContextName = p.ContextName,
                                   IDContext = p.IDContext,
                                   DataString = p.String,
                                   StringENG = p.String,
                                   IDContext2String = p.IDString2Context,
                                   StringType = p.StringType,
                                   StringTypeID = p.IDType
                               }
                               by p.LocalizationID;

                foreach (IGrouping<string, StringEntity> conList in Concepts)
                {
                    GroupedStringEntity sec = new GroupedStringEntity();
                    sec.Ignore = false;
                    sec.LocalizationID = conList.Key;
                    sec.ComponentNamespace = ComponentName;
                    sec.InternalNamespace = InternalNamespace;
                    sec.Group = conList.ToList();
                    sec.ConceptID = sec.Group[0].IDConcept;
                    retList.Add(sec);
                }
            }
            else
            {
                IEnumerable<GroupledData> dt1 = dbContext.GetComplimentaryDataByComponentInternalISOjob(idJobList, ComponentName, InternalNamespace, iso);
                var Concepts = from p in dt
                               group new StringEntity
                               {
                                   IDConcept = p.ID,
                                   IDString = 0,
                                   IDConcept2Context = p.IDConcept2Context,
                                   ContextName = p.ContextName,
                                   IDContext = p.IDContext,
                                   DataString = null,
                                   StringENG = p.String,
                                   IDContext2String = 0,
                                   StringType = p.StringType,
                                   StringTypeID = p.IDType
                               }
                               by p.LocalizationID;

                var locConcepts = from p in dt1
                                  group new StringEntity
                                  {
                                      IDConcept = p.ID,
                                      IDString = p.IDString,
                                      IDConcept2Context = p.IDConcept2Context,
                                      ContextName = p.ContextName,
                                      IDContext = p.IDContext,
                                      DataString = p.String,
                                      StringENG = null,
                                      IDContext2String = p.IDString2Context,
                                      StringType = p.StringType,
                                      StringTypeID = p.IDType
                                  }
                                  by p.LocalizationID;
                var keyCollection = from q in locConcepts
                                    select q.Key;
                var OrderConcept = from o in Concepts
                                   where !keyCollection.Contains(o.Key)
                                   select new GroupedStringEntity
                                   {
                                       Ignore = false,
                                       LocalizationID = o.Key,
                                       ComponentNamespace = ComponentName,
                                       InternalNamespace = InternalNamespace,
                                       Group = o.ToList(),
                                       ConceptID = o.FirstOrDefault().IDConcept,
                                   };
                var locOrderConcept = from o in locConcepts
                                      select new GroupedStringEntity
                                      {
                                          Ignore = false,
                                          LocalizationID = o.Key,
                                          ComponentNamespace = ComponentName,
                                          InternalNamespace = InternalNamespace,
                                          Group = o.ToList(),
                                          ConceptID = o.FirstOrDefault().IDConcept,
                                      };
                var totalConcepts = OrderConcept.Union(locOrderConcept).OrderBy(o => o.LocalizationID);
                retList.AddRange(totalConcepts.ToList());
            }
        }

        private void MLFillAllInternal(string ComponentName, ref List<MultiLangString> retList)
        {
            // loop su tutti gli internal
            UltraDBConcept.UltraDBConcept inConcept = new UltraDBConcept.UltraDBConcept(dbContext);
            List<DBInternalNameSpace> inList = inConcept.GetAllInternalNamebyComponent(ComponentName);
            foreach (DBInternalNameSpace db in inList)
            {
                if (db.InternalNamespace == "all") continue;
                MLFillByComponentNamespace(db.InternalNamespace, ComponentName, ref retList);
            }
        }

        private void MLFillByComponentNamespace(string InternalNamespace, string ComponentName, ref List<MultiLangString> retList)
        {
            if (InternalNamespace == null || InternalNamespace == "" || InternalNamespace == "null")
                InternalNamespace = null;
            IEnumerable<DataTableEditConcept> dt = dbContext.GetEditDataByComponentInternal(ComponentName, InternalNamespace);
            List<MultiLangString> tmpEntityList = new List<MultiLangString>();
            UltraDBExtendedStrings exs = new UltraDBExtendedStrings(dbContext);
            foreach (var row in dt)
            {
                MultiLangString s = new MultiLangString();
                s.MLString = new string[Enum.GetNames(typeof(UltraDBStrings.UltraDBStrings.Languages)).Length];
                s.ContextName = row.ContextName;
                s.LocalizationID = row.LocalizationID;
                s.ComponentNamespace = ComponentName;
                s.InternalNamespace = InternalNamespace;
                //row.ID concept2context
                foreach (UltraDBStrings.UltraDBStrings.Languages iso in Enum.GetValues(typeof(UltraDBStrings.UltraDBStrings.Languages)))
                {
                    // se il concept ha Ignore=true e non siamo in inglese, salta
                    if (row.Ignore && iso != UltraDBStrings.UltraDBStrings.Languages.en) continue;
                    DBExtendedStrings xstr = exs.GetStringByConcept2ContextISO(row.ID, iso.ToString());
                    if (xstr != null)
                    {
                        s.MLString[(int)iso - 1] = xstr.DataString;
                        s.StringType = xstr.StringType;
                        //xstr.IDString id della stringa
                    }
                }
                tmpEntityList.Add(s);
            }
            retList.AddRange(tmpEntityList);
        }

        private void FillByComponentNamespace(string InternalNamespace, string ComponentName, int iso, int idJobList, string strIso, ref List<GroupedStringEntity> retList)
        {
            // se non si fa il browse dell'intero db, usa il metodo + rapido
            if (idJobList != 0)
            {
                FillEngData(InternalNamespace, ComponentName, idJobList, iso, ref retList);
                return;
            }
            IEnumerable<DataTableEditConcept> dt;
            if (InternalNamespace == null || InternalNamespace == "" || InternalNamespace == "null")
                InternalNamespace = null;
            if (idJobList == 0)
                dt = dbContext.GetEditDataByComponentInternal(ComponentName, InternalNamespace);
            else
                dt = dbContext.GetDatabyComponentInternalJob(ComponentName, InternalNamespace, idJobList);
            List<StringEntity> tmpEntityList = new List<StringEntity>();
            UltraDBExtendedStrings exs = new UltraDBExtendedStrings(dbContext);
            foreach (var row in dt)
            {
                // se il concept ha Ignore=true e non siamo in inglese, salta
                if (row.Ignore && iso != 1) continue;
                DBExtendedStrings xstr = exs.GetStringByConcept2ContextISO(row.ID, strIso);
                DBExtendedStrings xstrENG = exs.GetStringByConcept2ContextISO(row.ID, "en");
                StringEntity s = new StringEntity();
                s.IDConcept = row.IDConcept;
                s.IDConcept2Context = row.ID;
                s.ContextName = row.ContextName;
                s.IDContext = row.IDContext;
                s.LocalizationID = row.LocalizationID;
                if (xstr != null)
                {
                    s.DataString = xstr.DataString;
                    s.IDString = xstr.IDString;
                    s.IDContext2String = xstr.IDContext2String;
                }
                if (xstrENG != null)
                {
                    s.StringENG = xstrENG.DataString;
                    s.StringTypeID = xstrENG.IDTYpe;
                    s.StringType = xstrENG.StringType;
                }
                tmpEntityList.Add(s);
            }
            var Concepts = from p in tmpEntityList
                           group new StringEntity
                           {
                               IDConcept = p.IDConcept,
                               IDString = p.IDString,
                               IDConcept2Context = p.IDConcept2Context,
                               ContextName = p.ContextName,
                               IDContext = p.IDContext,
                               DataString = p.DataString,
                               StringENG = p.StringENG,
                               IDContext2String = p.IDContext2String,
                               StringType = p.StringType,
                               StringTypeID = p.StringTypeID
                           }
                           by p.LocalizationID;

            foreach (IGrouping<string, StringEntity> conList in Concepts)
            {
                GroupedStringEntity sec = new GroupedStringEntity();
                sec.Ignore = false;
                sec.LocalizationID = conList.Key;
                sec.ComponentNamespace = ComponentName;
                sec.InternalNamespace = InternalNamespace;
                sec.Group = conList.ToList();
                sec.ConceptID = sec.Group[0].IDConcept;
                retList.Add(sec);
            }
        }

        #endregion
    }
}
