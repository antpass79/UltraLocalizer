using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class InternalConceptsTableTableAdapter
    {
        // SELECT DISTINCT InternalNamespace
        // FROM            LOC_ConceptsTable
        // WHERE(ComponentNamespace = @Component)
        // ORDER BY InternalNamespace
        public static List<InternalConceptsTable> GetInternalByComponent(this LocalizationContext context, string Component)
        {
            //var result = (from entity in context.LocConceptsTable
            //              where entity.ComponentNamespace == Component
            //              orderby entity.InternalNamespace
            //              select new InternalConceptsTable
            //              {
            //                  InternalNamespace = entity.InternalNamespace
            //              }).Distinct();

            var result = from entity in context.LocConceptsTables
                          select entity;

            if (!string.IsNullOrWhiteSpace(Component) && Component.ToLower() != "all")
                result = result.Where(entity => entity.ComponentNamespace == Component);

            return result
                .OrderBy(entity => entity.InternalNamespace)
                .Select(entity => new InternalConceptsTable
                {
                    InternalNamespace = entity.InternalNamespace
                })
                .Distinct()
                .ToList();
        }
    }
}
