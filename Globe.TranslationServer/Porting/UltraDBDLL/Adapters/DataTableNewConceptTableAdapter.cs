using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    internal static class DataTableNewConceptTableAdapter
    {
        // SELECT DISTINCT
        //      LOC_ConceptsTable.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID, 
        //      LOC_Concept2Context.IDContext,
        //      LOC_Concept2Context.ID AS IDConcept2Context,
        //      LOC_CONTEXTS.ContextName
        // FROM LOC_ConceptsTable
        // INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // CROSS JOIN LOC_Strings2Context
        // WHERE LOC_ConceptsTable.ComponentNamespace= @ComponentNamespace AND LOC_ConceptsTable.InternalNamespace IS NULL AND (LOC_Concept2Context.ID NOT IN
        //      (SELECT DISTINCT IDConcept2Context
        //       FROM LOC_Strings2Context AS LOC_Strings2Context_1))
        public static IEnumerable<DataTableNewConcept> GetNewConceptAndContextIDbyComponent(this LocalizationContext context, string ComponentName)
        {
            // ERROR IN THE PARAMETER NAME
            string query =
                    $@"
                        SELECT DISTINCT
                            LOC_ConceptsTable.ID,
                            LOC_ConceptsTable.ComponentNamespace,
                            LOC_ConceptsTable.InternalNamespace,
                            LOC_ConceptsTable.LocalizationID, 
                            LOC_Concept2Context.IDContext,
                            LOC_Concept2Context.ID AS IDConcept2Context,
                            LOC_CONTEXTS.ContextName
                        FROM LOC_ConceptsTable
                        INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
                        INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
                        CROSS JOIN LOC_Strings2Context
                        WHERE LOC_ConceptsTable.ComponentNamespace= {ComponentName} AND LOC_ConceptsTable.InternalNamespace IS NULL AND (LOC_Concept2Context.ID NOT IN
                            (SELECT DISTINCT IDConcept2Context
                            FROM LOC_Strings2Context AS LOC_Strings2Context_1))
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableNewConcept> result = new List<DataTableNewConcept>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableNewConcept
                    {
                        ID = (int)reader[0],
                        ComponentNamespace = reader[1] as string,
                        InternalNamespace = reader[2] as string,
                        LocalizationID = (int)reader[3],
                        IDContext = (int)reader[4],
                        IDConcept2Context = (int)reader[5],
                        ContextName = reader[6] as string,
                    });
                }
            }

            return result.ToList();
        }

        // SELECT DISTINCT
        //      LOC_ConceptsTable.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID, 
        //      LOC_Concept2Context.IDContext,
        //      LOC_Concept2Context.ID AS IDConcept2Context,
        //      LOC_CONTEXTS.ContextName
        // FROM LOC_ConceptsTable
        // INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // CROSS JOIN LOC_Strings2Context
        // WHERE LOC_ConceptsTable.ComponentNamespace= @ComponentNamespace AND LOC_ConceptsTable.InternalNamespace= @InternalNamespace AND (LOC_Concept2Context.ID NOT IN
        //      (SELECT DISTINCT IDConcept2Context
        //       FROM LOC_Strings2Context AS LOC_Strings2Context_1))
        public static IEnumerable<DataTableNewConcept> GetNewConceptAndContextIDbyComponentInternal(this LocalizationContext context, string ComponentName, string InternalNamespace)
        {
            string query =
                    $@"
                         SELECT DISTINCT
                              LOC_ConceptsTable.ID,
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace,
                              LOC_ConceptsTable.LocalizationID, 
                              LOC_Concept2Context.IDContext,
                              LOC_Concept2Context.ID AS IDConcept2Context,
                              LOC_CONTEXTS.ContextName
                         FROM LOC_ConceptsTable
                         INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
                         CROSS JOIN LOC_Strings2Context
                         WHERE LOC_ConceptsTable.ComponentNamespace= {ComponentName} AND LOC_ConceptsTable.InternalNamespace= {InternalNamespace} AND (LOC_Concept2Context.ID NOT IN
                              (SELECT DISTINCT IDConcept2Context
                               FROM LOC_Strings2Context AS LOC_Strings2Context_1))
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableNewConcept> result = new List<DataTableNewConcept>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableNewConcept
                    {
                        ID = (int)reader[0],
                        ComponentNamespace = reader[1] as string,
                        InternalNamespace = reader[2] as string,
                        LocalizationID = (int)reader[3],
                        IDContext = (int)reader[4],
                        IDConcept2Context = (int)reader[5],
                        ContextName = reader[6] as string,
                    });
                }
            }

            return result.ToList();
        }
    }
}
