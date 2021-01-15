using Globe.TranslationServer.Entities;
using System;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class String2ContextTableAdapter
    {
        public static void DeletebyIDStringIDConcept2Context(this LocalizationContext context, int idString, int idConcept2Context)
        {
            var itemToRemove = context.LocStrings2Contexts
                .AsQueryable()
                .Where(item => item.Idstring == idString && item.Idconcept2Context == idConcept2Context)
                .Single();
            context.LocStrings2Contexts.Remove(itemToRemove);
        }

        public static void InsertNewStrings2Context(this LocalizationContext context, int IDString, int IDConcept2Context)
        {
            context.LocStrings2Contexts.Add(new LocStrings2Context
            {
                Idstring = IDString,
                Idconcept2Context = IDConcept2Context
            });
        }
    }
}
