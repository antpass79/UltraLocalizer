using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class ConceptsTableTableAdapter
    {
        public static int InsertNewConcept(this LocalizationContext context, string ComponentNamespace, string InternalNamespace, string LocalizationID, bool Ignore, string Comment)
        {
            var item = new LocConceptsTable
            {
                ComponentNamespace = ComponentNamespace,
                InternalNamespace = InternalNamespace,
                LocalizationId = LocalizationID,
                Ignore = Ignore,
                Comment = Comment
            };

            context.LocConceptsTables.Add(item);

            context.SaveChanges();

            return item.Id;
        }

        public static void UpdateConcept(this LocalizationContext context, int ID, bool Ignore, string Comment)
        {
            var concept = context.LocConceptsTables.Find(ID);
            concept.Ignore = Ignore;
            concept.Comment = Comment;
        }

        public static void CleanOrphanedConcepts(this LocalizationContext context)
        {
            throw new NotImplementedException(nameof(CleanOrphanedConcepts));
        }

        //        SELECT LOC_ConceptsTable.*
        //FROM LOC_ConceptsTable
        public static IQueryable<ConceptsTable> GetData(this LocalizationContext context)
        {
            var result = (from entity in context.LocConceptsTables
                          select new ConceptsTable
                          {
                              ID = entity.Id,
                              ComponentNamespace = entity.ComponentNamespace,
                              InternalNamespace = entity.InternalNamespace,
                              LocalizationID = entity.LocalizationId,
                              Ignore = entity.Ignore.HasValue ? entity.Ignore.Value : false,
                              Comment = entity.Comment,
                              SSMA_TimeStamp = entity.SsmaTimeStamp                             
                          });

            return result;
        }

        //        SELECT LOC_ConceptsTable.*
        //FROM LOC_ConceptsTable
        //where LOC_ConceptsTable.ID = @ID
        public static ConceptsTable GetDataByID(this LocalizationContext context, int id)
        {
            var result = (from entity in context.LocConceptsTables
                          where entity.Id == id
                          select new ConceptsTable
                          {
                              ID = entity.Id,
                              ComponentNamespace = entity.ComponentNamespace,
                              InternalNamespace = entity.InternalNamespace,
                              LocalizationID = entity.LocalizationId,
                              Comment = entity.Comment,
                              Ignore = entity.Ignore.HasValue ? entity.Ignore.Value : false,
                              SSMA_TimeStamp = entity.SsmaTimeStamp
                          }).FirstOrDefault();

            return result;
        }
    }
}
