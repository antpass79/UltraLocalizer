using Globe.TranslationServer.Entities;
using System;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class LOC_Job2ConceptTableAdapter
    {
        public static void AppendConcept2JobList(this LocalizationContext context, int idJobList, int idConcept2Context)
        {
            // ANTO not found in the original code
            throw new NotImplementedException();
        }

        //DELETE FROM[dbo].[LOC_Job2Concept] WHERE[IDJobList] = @IDJobList
        public static void DeleteJob2Concept(this LocalizationContext context, int idJobList)
        {
            var itemToRemove = context.LocJob2Concepts.Find(idJobList);
            context.LocJob2Concepts.Remove(itemToRemove);
            context.SaveChanges();
        }
    }
}
