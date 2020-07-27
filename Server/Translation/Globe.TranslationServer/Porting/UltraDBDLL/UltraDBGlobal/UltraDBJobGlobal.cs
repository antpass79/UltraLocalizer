using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal
{
    public class UltraDBJobGlobal
    {
        private readonly LocalizationContext context;

        public UltraDBJobGlobal(LocalizationContext context)
        {
            this.context = context;
        }

        #region Public Functions

        public List<JobComponent> GetMissingDataBy(string isocoding)
        {
            List<JobComponent> retList = new List<JobComponent>();
            // loop su tutti i componenti
            UltraDBConcept.UltraDBConcept inComponent = new UltraDBConcept.UltraDBConcept(context);
            List<DBComponent> comList = inComponent.GetAllComponent();
            foreach (DBComponent db in comList)
            {
                if (db.ComponentNamespace == "OLD") continue;
                if (db.ComponentNamespace == "all") continue;
                List<JobInternal> interlist = FillAllInternal(db.ComponentNamespace, isocoding);
                if (interlist.Count > 0)
                {
                    JobComponent jb = new JobComponent();
                    jb.ComponentNamespace = db.ComponentNamespace;
                    jb.InternalName = interlist;
                    retList.Add(jb);
                }
            }
            return retList;
        }

        public List<JobGroupedStringEntity> FillByComponentNamespace(string InternalNamespace, string ComponentName, string isocoding)
        {
            if (isocoding == "en")
                return FillByComponentNamespaceEng(InternalNamespace, ComponentName);
            IEnumerable<DataTableGlobal> dt;
            List<JobGroupedStringEntity> retList = new List<JobGroupedStringEntity>();
            if (InternalNamespace == null || InternalNamespace == "" || InternalNamespace == "null")
                dt = context.GetMissingDataByComponentISO(ComponentName, isocoding);
            else
                dt = context.GetMissingDataByComponentISOInternal(ComponentName, InternalNamespace, isocoding);
            var Concepts = from p in dt
                           group new JobStringEntity { IDConcept2Context = p.ID, ContextName = p.ContextName, IDConcept = p.ConceptID }
                           by p.LocalizationID;

            foreach (IGrouping<string, JobStringEntity> conList in Concepts)
            {
                JobGroupedStringEntity sec = new JobGroupedStringEntity();
                sec.LocalizationID = conList.Key;
                sec.ComponentNamespace = ComponentName;
                sec.InternalNamespace = InternalNamespace;
                sec.Group = conList.ToList();
                sec.ConceptID = sec.Group[0].IDConcept;
                retList.Add(sec);
            }
            return retList;
        }

        public List<JobGroupedStringEntity> FillByComponentNamespaceEng(string InternalNamespace, string ComponentName)
        {
            List<JobGroupedStringEntity> retList = new List<JobGroupedStringEntity>();
            IEnumerable<DataTableNewConcept> dt;
            if (InternalNamespace == null || InternalNamespace == "" || InternalNamespace == "null")
                dt = context.GetNewConceptAndContextIDbyComponent(ComponentName);
            else
                dt = context.GetNewConceptAndContextIDbyComponentInternal(ComponentName, InternalNamespace);
            var Concepts = from p in dt
                           group new JobStringEntity { IDConcept2Context = p.IDConcept2Context, ContextName = p.ContextName, IDConcept = p.ID }
                           by p.LocalizationID;

            foreach (IGrouping<string, JobStringEntity> conList in Concepts)
            {
                JobGroupedStringEntity sec = new JobGroupedStringEntity();
                sec.LocalizationID = conList.Key;
                sec.ComponentNamespace = ComponentName;
                sec.InternalNamespace = InternalNamespace;
                sec.Group = conList.ToList();
                sec.ConceptID = sec.Group[0].IDConcept;
                retList.Add(sec);
            }
            return retList;
        }

        #endregion

        #region Private Functions

        private List<JobInternal> FillAllInternal(string ComponentName, string isocoding)
        {
            // loop su tutti gli internal
            List<JobInternal> retList = new List<JobInternal>();
            UltraDBConcept.UltraDBConcept concept = new UltraDBConcept.UltraDBConcept(context);
            List<DBInternalNameSpace> inList = concept.GetAllInternalNamebyComponent(ComponentName);
            foreach (DBInternalNameSpace db in inList)
            {
                if (db.InternalNamespace == "all") continue;
                List<JobGroupedStringEntity> retLst = FillByComponentNamespace(db.InternalNamespace, ComponentName, isocoding);
                if (retLst.Count > 0)
                {
                    JobInternal jb = new JobInternal();
                    jb.ComponentNamespace = ComponentName;
                    jb.InternalNamespace = db.InternalNamespace;
                    retList.Add(jb);
                }
            }
            return retList;
        }

        #endregion
    }
}
