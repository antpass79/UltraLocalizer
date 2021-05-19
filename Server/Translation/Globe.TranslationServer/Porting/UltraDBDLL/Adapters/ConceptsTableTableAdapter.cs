using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class ConceptsTableTableAdapter
    {
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
