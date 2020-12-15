//using Globe.TranslationServer.DTOs;
//using Globe.TranslationServer.Entities;
//using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
//using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
//using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
//using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
//using Globe.TranslationServer.Services;
//using Globe.TranslationServer.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal
//{
//    public class UltraDBJobGlobal_EF : IUltraDBJobGlobal
//    {
//        private readonly LocalizationContext _context;

//        public UltraDBJobGlobal_EF(LocalizationContext context)
//        {
//            _context = context;
//        }

//        #region Public Functions

//        public List<InternalNamespaceGroupDTO> GetMissingDataBy(string isoCoding)
//        {
//            if (isoCoding == Constants.LANGUAGE_EN)
//                return FillByComponentNamespaceEng();

//            IEnumerable<DataTableGlobal> result;
//            List<JobGroupedStringEntity> retList = new List<JobGroupedStringEntity>();

//            var subQuery = _context.VStringsToContext
//                .Where(item => item.Isocoding == isoCoding)
//                .Select(item => item.Id);

//            result = _context.VStringsToContext
//                .Where(item => item.Ignore.HasValue && !item.Ignore.Value && item.Isocoding == Constants.LANGUAGE_EN && !subQuery.Contains(item.Id))
//                .Select(item => new DataTableGlobal
//                {
//                    ID = item.Id,
//                    ComponentNamespace = item.ComponentNamespace,
//                    InternalNamespace = item.InternalNamespace,
//                    LocalizationID = item.LocalizationId,
//                    ISOCoding = item.Isocoding,
//                    ContextName = item.ContextName,
//                    String = item.String,
//                    IDType = item.Idtype,
//                    Type = item.Type,
//                    StringID = item.StringId,
//                    Ignore = item.Ignore.HasValue && item.Ignore.Value,
//                    ConceptID = item.ConceptId,
//                })
//                .ToList();

//            var Concepts = from p in result
//                           group new JobStringEntity
//                           {
//                               IDConcept2Context = p.ID,
//                               ContextName = p.ContextName,
//                               IDConcept = p.ConceptID
//                           }
//                           by p.LocalizationID;

//            foreach (IGrouping<string, JobStringEntity> conList in Concepts)
//            {
//                JobGroupedStringEntity sec = new JobGroupedStringEntity();
//                sec.LocalizationID = conList.Key;
//                sec.ComponentNamespace = componentName;
//                sec.InternalNamespace = internalNamespace;
//                sec.Group = conList.ToList();
//                sec.ConceptID = sec.Group[0].IDConcept;
//                retList.Add(sec);
//            }
//            return retList;
//        }

//        public List<JobGroupedStringEntity> FillByComponentNamespace(string internalNamespace, string componentName, string isoCoding)
//        {
//            if (isoCoding == Constants.LANGUAGE_EN)
//                return FillByComponentNamespaceEng(internalNamespace, componentName);

//            IEnumerable<DataTableGlobal> result;
//            List<JobGroupedStringEntity> retList = new List<JobGroupedStringEntity>();
//            if (!CheckInternalNamespace(internalNamespace))
//            {
//                var subQuery = _context.VStringsToContext
//                    .Where(item => item.ComponentNamespace == componentName && item.InternalNamespace == null && item.Isocoding == isoCoding)
//                    .Select(item => item.Id);

//                result = _context.VStringsToContext
//                    .Where(item => item.Ignore.HasValue && !item.Ignore.Value && item.ComponentNamespace == componentName && item.InternalNamespace == null && item.Isocoding == Constants.LANGUAGE_EN && !subQuery.Contains(item.Id))
//                    .Select(item => new DataTableGlobal
//                    {
//                        ID = item.Id,
//                        ComponentNamespace = item.ComponentNamespace,
//                        InternalNamespace = item.InternalNamespace,
//                        LocalizationID = item.LocalizationId,
//                        ISOCoding = item.Isocoding,
//                        ContextName = item.ContextName,
//                        String = item.String,
//                        IDType = item.Idtype,
//                        Type = item.Type,
//                        StringID = item.StringId,
//                        Ignore = item.Ignore.HasValue && item.Ignore.Value,
//                        ConceptID = item.ConceptId,
//                    })
//                    .ToList();
//            }
//            else
//            {
//                var subQuery = _context.VStringsToContext
//                    .Where(item => item.ComponentNamespace == componentName && item.InternalNamespace == internalNamespace && item.Isocoding == isoCoding)
//                    .Select(item => item.Id);

//                result = _context.VStringsToContext
//                    .Where(item => item.Ignore.HasValue && !item.Ignore.Value && item.ComponentNamespace == componentName && item.InternalNamespace == internalNamespace && item.Isocoding == Constants.LANGUAGE_EN && !subQuery.Contains(item.Id))
//                    .Select(item => new DataTableGlobal
//                    {
//                        ID = item.Id,
//                        ComponentNamespace = item.ComponentNamespace,
//                        InternalNamespace = item.InternalNamespace,
//                        LocalizationID = item.LocalizationId,
//                        ISOCoding = item.Isocoding,
//                        ContextName = item.ContextName,
//                        String = item.String,
//                        IDType = item.Idtype,
//                        Type = item.Type,
//                        StringID = item.StringId,
//                        Ignore = item.Ignore.HasValue && item.Ignore.Value,
//                        ConceptID = item.ConceptId,
//                    })
//                    .ToList();
//            }
//            var Concepts = from p in result
//                           group new JobStringEntity { IDConcept2Context = p.ID, ContextName = p.ContextName, IDConcept = p.ConceptID }
//                           by p.LocalizationID;

//            foreach (IGrouping<string, JobStringEntity> conList in Concepts)
//            {
//                JobGroupedStringEntity sec = new JobGroupedStringEntity();
//                sec.LocalizationID = conList.Key;
//                sec.ComponentNamespace = componentName;
//                sec.InternalNamespace = internalNamespace;
//                sec.Group = conList.ToList();
//                sec.ConceptID = sec.Group[0].IDConcept;
//                retList.Add(sec);
//            }
//            return retList;
//        }

//        public List<InternalNamespaceGroupDTO> FillByComponentNamespaceEng()
//        {
//            var internalNamespaceGroups = new List<InternalNamespaceGroupDTO>();

//            var groups = _context.VConceptStringToContext
//                .Where(item => item.Idstring == null)
//                .GroupBy(item => item.ComponentNamespace)
//                .Distinct();

//            foreach (IGrouping<string, VConceptStringToContext> group in groups)
//            {
//                var internalNamespaces = new List<InternalNamespaceDTO>();
//                group
//                    .ToList()
//                    .ForEach(item => internalNamespaces.Add(new InternalNamespaceDTO
//                    {
//                        Description = item.InternalNamespace
//                    }));
//                var internalNamespace = new InternalNamespaceGroupDTO
//                {
//                    ComponentNamespace = new ComponentNamespaceDTO { Description = group.Key },
//                    InternalNamespaces = internalNamespaces
//                };
//            }

//            return internalNamespaceGroups;
//        }

//        public List<JobGroupedStringEntity> FillByComponentNamespaceEng(string internalNamespace, string componentName)
//        {
//            List<JobGroupedStringEntity> toTranslateConcepts = new List<JobGroupedStringEntity>();
//            IEnumerable<DataTableNewConcept> newConcepts;

//            if (!CheckInternalNamespace(internalNamespace))
//            {
//                newConcepts = _context.VConceptStringToContext
//                    .Where(item => item.ComponentNamespace == componentName && item.Idstring == null)
//                    .Select(item => new DataTableNewConcept
//                    {
//                        ComponentNamespace = item.ComponentNamespace,
//                        ContextName = item.ContextName,
//                        ID = item.Id,
//                        IDConcept2Context = item.Idconcept2Context,
//                        IDContext = item.Idcontext,
//                        InternalNamespace = item.InternalNamespace,
//                        LocalizationID = item.LocalizationId
//                    })
//                    .ToList();
//            }
//            else
//            {
//                newConcepts = _context.VConceptStringToContext
//                    .Where(item => item.ComponentNamespace == componentName && item.InternalNamespace == internalNamespace && item.Idstring == null)
//                    .Select(item => new DataTableNewConcept
//                    {
//                        ComponentNamespace = item.ComponentNamespace,
//                        ContextName = item.ContextName,
//                        ID = item.Id,
//                        IDConcept2Context = item.Idconcept2Context,
//                        IDContext = item.Idcontext,
//                        InternalNamespace = item.InternalNamespace,
//                        LocalizationID = item.LocalizationId
//                    })
//                    .ToList();
//            }

//            var concepts = from newConcept in newConcepts
//                           group new JobStringEntity { IDConcept2Context = newConcept.IDConcept2Context, ContextName = newConcept.ContextName, IDConcept = newConcept.ID }
//                           by newConcept.LocalizationID;

//            foreach (IGrouping<string, JobStringEntity> concept in concepts)
//            {
//                JobGroupedStringEntity sec = new JobGroupedStringEntity();
//                sec.LocalizationID = concept.Key;
//                sec.ComponentNamespace = componentName;
//                sec.InternalNamespace = internalNamespace;
//                sec.Group = concept.ToList();
//                sec.ConceptID = sec.Group[0].IDConcept;
//                toTranslateConcepts.Add(sec);
//            }
//            return toTranslateConcepts;
//        }

//        #endregion

//        #region Private Functions

//        private bool CheckInternalNamespace(string internalNamespace)
//        {
//            return !(internalNamespace == null || internalNamespace == "" || internalNamespace == "null");
//        }

//        private List<JobInternal> FillAllInternal(string ComponentName, string isocoding)
//        {
//            // loop su tutti gli internal
//            List<JobInternal> retList = new List<JobInternal>();
//            UltraDBConcept.UltraDBConcept concept = new UltraDBConcept.UltraDBConcept(_context);
//            List<DBInternalNameSpace> inList = concept.GetAllInternalNamebyComponent(ComponentName);
//            foreach (DBInternalNameSpace db in inList)
//            {
//                if (db.InternalNamespace == "all") continue;
//                List<JobGroupedStringEntity> retLst = FillByComponentNamespace(db.InternalNamespace, ComponentName, isocoding);
//                if (retLst.Count > 0)
//                {
//                    JobInternal jb = new JobInternal();
//                    jb.ComponentNamespace = ComponentName;
//                    jb.InternalNamespace = db.InternalNamespace;
//                    retList.Add(jb);
//                }
//            }
//            return retList;
//        }

//        #endregion
//    }
//}
