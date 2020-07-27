using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal
{
    public class UltraDBNewConcept
    {
        private readonly LocalizationContext context;

        public UltraDBNewConcept(LocalizationContext context)
        {
            this.context = context;
        }

        public List<GroupedStringEntity> GetGroupledNewDataBy(string ComponentName, string InternalNamespace)
        {
            List<GroupedStringEntity> retList = new List<GroupedStringEntity>();
            if (ComponentName == "all")
            {
                // loop su tutti i componenti
                UltraDBConcept.UltraDBConcept inComponent = new UltraDBConcept.UltraDBConcept(context);
                List<DBComponent> comList = inComponent.GetAllComponent();
                foreach (DBComponent db in comList)
                {
                    if (db.ComponentNamespace == "OLD") continue;
                    if (db.ComponentNamespace == "all") continue;
                    FillAllNewInternal(db.ComponentNamespace, retList);
                }
            }
            else
            {
                if (InternalNamespace == "all")
                {
                    FillAllNewInternal(ComponentName, retList);
                }
                else
                    FillNewByComponentNamespace(InternalNamespace, ComponentName, ref retList);
            }
            return retList;
        }

        private void FillAllNewInternal(string ComponentName, List<GroupedStringEntity> retList)
        {
            // loop su tutti gli internal
            UltraDBConcept.UltraDBConcept inConcept = new UltraDBConcept.UltraDBConcept(context);
            List<DBInternalNameSpace> inList = inConcept.GetAllInternalNamebyComponent(ComponentName);
            foreach (DBInternalNameSpace db in inList)
            {
                if (db.InternalNamespace == "all") continue;
                FillNewByComponentNamespace(db.InternalNamespace, ComponentName, ref retList);
            }
        }
        private void FillNewByComponentNamespace(string InternalNamespace, string ComponentName, ref List<GroupedStringEntity> retList)
        {
            IEnumerable<DataTableNewConcept> dt;
            if (InternalNamespace == null || InternalNamespace == "" || InternalNamespace == "null")
                dt = context.GetNewConceptAndContextIDbyComponent(ComponentName);
            else
                dt = context.GetNewConceptAndContextIDbyComponentInternal(ComponentName, InternalNamespace);
            var Concepts = from p in dt
                           group new StringEntity { IDConcept = p.ID, IDConcept2Context = p.IDConcept2Context, ContextName = p.ContextName, IDContext = p.IDContext }
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
    }
}
