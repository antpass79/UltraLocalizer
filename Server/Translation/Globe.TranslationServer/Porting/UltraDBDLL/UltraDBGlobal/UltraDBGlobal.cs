using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal
{
    public class UltraDBGlobal
    {
        private readonly LocalizationContext context;

        public UltraDBGlobal(LocalizationContext context)
        {
            this.context = context;
        }

        public static int GetContextId(string contextName)
        {
            CONTEXTS eC = CONTEXTS.Generic_Medium;
            try
            {
                eC = (CONTEXTS)Enum.Parse(typeof(CONTEXTS), contextName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (int)eC;
        }

        public static string GetContextName(int id)
        {
            string eC = CONTEXTS.Generic_Medium.ToString();
            try
            {
                eC = Enum.GetName(typeof(CONTEXTS), id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return eC;
        }

        public void DeletebyIDStringIDConcept2Context(int idString, int idConcept2Context)
        {
            context.DeletebyIDStringIDConcept2Context(idString, idConcept2Context);
        }

        public int InsertNewStrings2Context(int IDString, int IDConcept2Context)
        {
            return context.InsertNewStrings2Context(IDString, IDConcept2Context);
        }

        public List<DBGlobal> GetDataByComponentISO(string ComponentName, string isocoding)
        {
            List<DBGlobal> retList = new List<DBGlobal>();
            var dt = context.GetDatabyComponentISO(ComponentName, isocoding);
            var dten = context.GetDatabyComponentISO(ComponentName, "en");
            foreach (var r in dten)
            {
                DBGlobal item = new DBGlobal();
                item.ComponentNamespace = r.ComponentNamespace;
                item.ContextName = r.ContextName;
                item.DataString = r.String;
                item.InternalNamespace = r.InternalNamespace;
                item.ISOCoding = isocoding;
                item.LocalizationID = r.LocalizationID;
                item.DatabaseID = r.StringID;
                item.IsAcceptable = r.IsAcceptable;
                item.Concept2ContextID = r.ID;
                item.IsToIgnore = r.Ignore;
                retList.Add(item);
            }

            if (isocoding == "en")
                return retList;
            // Replace all localized strings
            if (ComponentName == "MeasureComponent")
            {
                var z = from k in dten
                        where k.ContextName == "MD_RT11ExtLabel"
                        select k;
                foreach (var rz in z)
                {
                    DBGlobal item = retList.FirstOrDefault(t => t.ComponentNamespace == rz.ComponentNamespace &&
                                                                t.InternalNamespace == rz.InternalNamespace &&
                                                                t.LocalizationID == rz.LocalizationID &&
                                                                t.ContextName == rz.ContextName);
                    var item2 = dt.FirstOrDefault(t => t.ComponentNamespace == rz.ComponentNamespace &&
                                                                t.InternalNamespace == rz.InternalNamespace &&
                                                                t.LocalizationID == rz.LocalizationID &&
                                                                t.ContextName == rz.ContextName);
                    if (item != null && item2 == null)
                    {
                        var itemFallback = dt.FirstOrDefault(t => t.ComponentNamespace == rz.ComponentNamespace &&
                                                                    t.InternalNamespace == rz.InternalNamespace &&
                                                                    t.LocalizationID == rz.LocalizationID &&
                                                                    t.ContextName == "MD_RT11MeasName");
                        if (itemFallback != null)
                        {
                            item.DataString = itemFallback.String;
                            item.DatabaseID = itemFallback.StringID;
                            item.IsAcceptable = itemFallback.IsAcceptable;
                            item.Concept2ContextID = itemFallback.ID;
                        }
                    }
                }
            }
            foreach (var r in dt)
            {
                DBGlobal item = retList.FirstOrDefault(t => t.ComponentNamespace == r.ComponentNamespace &&
                                                            t.InternalNamespace == r.InternalNamespace &&
                                                            t.LocalizationID == r.LocalizationID &&
                                                            t.ContextName == r.ContextName);
                if (item != null)
                {
                    item.DataString = r.String;
                    item.DatabaseID = r.StringID;
                    item.IsAcceptable = r.IsAcceptable;
                    item.Concept2ContextID = r.ID;
                }
            }
            return retList;
        }


        public List<GroupedStringEntity> GetGroupledMissingDataBy(string ComponentName, string InternalNamespace, string isocoding)
        {
            List<GroupedStringEntity> retList = new List<GroupedStringEntity>();
            if (ComponentName == "all")
            {
                // loop su tutti i componenti
                UltraDBConcept.UltraDBConcept concept = new UltraDBConcept.UltraDBConcept(context);
                List<DBComponent> comList = concept.GetAllComponent();
                foreach (DBComponent db in comList)
                {
                    if (db.ComponentNamespace == "OLD") continue;
                    if (db.ComponentNamespace == "all") continue;
                    FillAllInternal(db.ComponentNamespace, isocoding, retList);
                }
            }
            else
            {
                if (InternalNamespace == "all")
                {
                    FillAllInternal(ComponentName, isocoding, retList);
                }
                else
                    FillByComponentNamespace(InternalNamespace, ComponentName, isocoding, ref retList);
            }
            return retList;
        }

        public List<StringEntity> GetStrings2KeepInEnglish(int ConceptID, string isocoding)
        {
            var dt = context.GetMissingDataByConceptID(ConceptID, isocoding);

            List<StringEntity> retList = (from p in dt
                                          select new StringEntity { IDConcept2Context = p.ID, StringTypeID = p.IDType, ContextName = p.ContextName, DataString = p.String, StringType = p.Type, Ignore = p.Ignore, IDConcept = p.ConceptID }).Distinct().ToList();
            return retList;
        }

        public List<int> GetMissingSiblings(int stringID, int isocoding)
        {
            var dt = context.GetSiblingsByIDStringISO(stringID, isocoding);
            List<int> retList = dt.Distinct().ToList();
            return retList;
        }

        #region Private Functions

        private void FillAllInternal(string ComponentName, string isocoding, List<GroupedStringEntity> retList)
        {
            // loop su tutti gli internal
            UltraDBConcept.UltraDBConcept concept = new UltraDBConcept.UltraDBConcept(context);
            List<DBInternalNameSpace> inList = concept.GetAllInternalNamebyComponent(ComponentName);
            foreach (DBInternalNameSpace db in inList)
            {
                if (db.InternalNamespace == "all") continue;
                FillByComponentNamespace(db.InternalNamespace, ComponentName, isocoding, ref retList);
            }
        }
        private void FillByComponentNamespace(string InternalNamespace, string ComponentName, string isocoding, ref List<GroupedStringEntity> retList)
        {
            IEnumerable<DataTableGlobal> dt;
            if (InternalNamespace == null || InternalNamespace == "" || InternalNamespace == "null")
                dt = context.GetMissingDataByComponentISO(ComponentName, isocoding);
            else
                dt = context.GetMissingDataByComponentISOInternal(ComponentName, InternalNamespace, isocoding);
            var Concepts = from p in dt
                           group new StringEntity { IDConcept2Context = p.ID, StringTypeID = p.IDType, ContextName = p.ContextName, DataString = p.String, StringType = p.Type, Ignore = p.Ignore, IDConcept = p.ConceptID, IDString = p.ID }
                           by p.LocalizationID;

            foreach (IGrouping<string, StringEntity> conList in Concepts)
            {
                GroupedStringEntity sec = new GroupedStringEntity();
                sec.LocalizationID = conList.Key;
                sec.ComponentNamespace = ComponentName;
                sec.InternalNamespace = InternalNamespace;
                sec.Group = conList.ToList();
                sec.ConceptID = sec.Group[0].IDConcept;
                sec.Ignore = sec.Group[0].Ignore;
                retList.Add(sec);
            }
        }

        #endregion
    }
}
