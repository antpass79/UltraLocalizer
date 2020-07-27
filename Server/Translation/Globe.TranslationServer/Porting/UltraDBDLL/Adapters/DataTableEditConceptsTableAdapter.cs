using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class DataTableEditConceptsTableAdapter
    {
        // SELECT
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_ConceptsTable.Ignore,
        //      LOC_ConceptsTable.ID AS IDConcept,
        //      LOC_Concept2Context.IDContext,
        //      LOC_Concept2Context.ID,
        //      LOC_ConceptsTable.Comment
        // FROM LOC_Concept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context
        // WHERE (LOC_Job2Concept.IDJobList = @IDJobList)
        public static IEnumerable<DataTableEditConcept> GetDataByJob(this LocalizationContext context, int IDJobList)
        {
            string query =
                    $@"
                         SELECT
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace,
                              LOC_ConceptsTable.LocalizationID,
                              LOC_CONTEXTS.ContextName,
                              LOC_ConceptsTable.Ignore,
                              LOC_ConceptsTable.ID AS IDConcept,
                              LOC_Concept2Context.IDContext,
                              LOC_Concept2Context.ID,
                              LOC_ConceptsTable.Comment
                         FROM LOC_Concept2Context
                         INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
                         INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context
                         WHERE (LOC_Job2Concept.IDJobList = '{IDJobList}')
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableEditConcept> result = new List<DataTableEditConcept>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableEditConcept
                    {
                        ComponentNamespace = reader[0] as string,
                        InternalNamespace = reader[1] as string,
                        LocalizationID = reader[2] as string,
                        ContextName = reader[3] as string,
                        Ignore = (bool)reader[4],
                        IDConcept = (int)reader[5],
                        IDContext = (int)reader[6],
                        ID = (int)reader[7],
                        Comment = reader[8] as string,
                    });
                }
            }

            return result.ToList();
        }

        // SELECT
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_ConceptsTable.Ignore,
        //      LOC_ConceptsTable.ID AS IDConcept,
        //      LOC_Concept2Context.IDContext,
        //      LOC_Concept2Context.ID,
        //      LOC_ConceptsTable.Comment
        // FROM LOC_Concept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // WHERE (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace = @Internal) OR
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace IS NULL) AND
        //       (@Internal IS NULL)
        public static IEnumerable<DataTableEditConcept> GetEditDataByComponentInternal(this LocalizationContext context, string ComponentName, string InternalNamespace)
        {
            string query =
                    $@"
                         SELECT
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace,
                              LOC_ConceptsTable.LocalizationID,
                              LOC_CONTEXTS.ContextName,
                              LOC_ConceptsTable.Ignore,
                              LOC_ConceptsTable.ID AS IDConcept,
                              LOC_Concept2Context.IDContext,
                              LOC_Concept2Context.ID,
                              LOC_ConceptsTable.Comment
                         FROM LOC_Concept2Context
                         INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
                         WHERE (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace = '{InternalNamespace}') OR
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace IS NULL) AND
                               ('{InternalNamespace}' IS NULL)
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableEditConcept> result = new List<DataTableEditConcept>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableEditConcept
                    {
                        ComponentNamespace = reader[0] as string,
                        InternalNamespace = reader[1] as string,
                        LocalizationID = reader[2] as string,
                        ContextName = reader[3] as string,
                        Ignore = (bool)reader[4],
                        IDConcept = (int)reader[5],
                        IDContext = (int)reader[6],
                        ID = (int)reader[7],
                        Comment = reader[8] as string,
                    });
                }
            }

            return result.ToList();
        }

        // SELECT
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_ConceptsTable.Ignore,
        //      LOC_ConceptsTable.ID AS IDConcept,
        //      LOC_Concept2Context.IDContext,
        //      LOC_Concept2Context.ID,
        //      LOC_ConceptsTable.Comment
        // FROM LOC_Concept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context
        // WHERE (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace = @Internal) AND
        //       (LOC_Job2Concept.IDJobList = @IDJobList) OR
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND(LOC_ConceptsTable.InternalNamespace IS NULL) AND
        //       (LOC_Job2Concept.IDJobList = @IDJobList) AND
        //       (@Internal IS NULL)
        public static IEnumerable<DataTableEditConcept> GetDatabyComponentInternalJob(this LocalizationContext context, string ComponentName, string InternalNamespace, int idJobList)
        {
            string query =
                    $@"
                         SELECT
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace,
                              LOC_ConceptsTable.LocalizationID,
                              LOC_CONTEXTS.ContextName,
                              LOC_ConceptsTable.Ignore,
                              LOC_ConceptsTable.ID AS IDConcept,
                              LOC_Concept2Context.IDContext,
                              LOC_Concept2Context.ID,
                              LOC_ConceptsTable.Comment
                         FROM LOC_Concept2Context
                         INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
                         INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context
                         WHERE (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace = '{InternalNamespace}') AND
                               (LOC_Job2Concept.IDJobList = '{idJobList}') OR
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND(LOC_ConceptsTable.InternalNamespace IS NULL) AND
                               (LOC_Job2Concept.IDJobList = '{idJobList}') AND
                               ('{InternalNamespace}' IS NULL)
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableEditConcept> result = new List<DataTableEditConcept>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableEditConcept
                    {
                        ComponentNamespace = reader[0] as string,
                        InternalNamespace = reader[1] as string,
                        LocalizationID = reader[2] as string,
                        ContextName = reader[3] as string,
                        Ignore = (bool)reader[4],
                        IDConcept = (int)reader[5],
                        IDContext = (int)reader[6],
                        ID = (int)reader[7],
                        Comment = reader[8] as string,
                    });
                }
            }

            return result.ToList();
        }
    }
}
