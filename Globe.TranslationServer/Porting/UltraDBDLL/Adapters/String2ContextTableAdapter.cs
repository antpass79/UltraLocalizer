using Globe.TranslationServer.Entities;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    internal static class String2ContextTableAdapter
    {
        public static void DeletebyIDStringIDConcept2Context(this LocalizationContext context, int idString, int idConcept2Context)
        {
            var itemToRemove = context.LocStrings2Context.Find(idString, idConcept2Context);
            context.LocStrings2Context.Remove(itemToRemove);
            context.SaveChanges();
        }

        public static int InsertNewStrings2Context(this LocalizationContext context, int IDString, int IDConcept2Context)
        {
            context.LocStrings2Context.Add(new LocStrings2Context
            {
                Idstring = IDString,
                Idconcept2Context = IDConcept2Context
            });
            return context.SaveChanges();
        }
    }
}
