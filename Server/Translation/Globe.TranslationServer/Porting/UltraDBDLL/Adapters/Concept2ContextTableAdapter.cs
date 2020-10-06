using Globe.TranslationServer.Entities;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class Concept2ContextTableAdapter
    {
        public static void InsertNewConcept2Context(this LocalizationContext context, int IDConcept, int IDContext)
        {
            context.LocConcept2Context.Add(new LocConcept2Context
            {
                Idconcept = IDConcept,
                Idcontext = IDContext
            });

            context.SaveChanges();
        }
    }
}
