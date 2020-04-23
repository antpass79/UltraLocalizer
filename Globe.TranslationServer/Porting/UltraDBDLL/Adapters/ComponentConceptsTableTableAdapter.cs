using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class ComponentConceptsTableTableAdapter
    {
        // SELECT DISTINCT ComponentNamespace
        // FROM            LOC_ConceptsTable
        // ORDER BY ComponentNamespace
        public static List<ComponentConceptsTable> GetAllComponentName(this LocalizationContext context)
        {
            var result = (from entity in context.LocConceptsTable
                          orderby entity.ComponentNamespace
                          select new ComponentConceptsTable
                          {
                              ComponentNamespace = entity.ComponentNamespace
                          }).Distinct();

            return result.ToList();
        }
    }
}
