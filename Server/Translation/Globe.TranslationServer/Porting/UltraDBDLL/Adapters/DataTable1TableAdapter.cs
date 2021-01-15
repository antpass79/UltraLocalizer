using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    // namespace DataSetConceptAndContextTableAdapters
    public static class DataTable1TableAdapter
    {
        // SELECT
        //      LOC_ConceptsTable.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_ConceptsTable.Comment,
        //      LOC_Concept2Context.ID AS IDConcept2Context,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_Concept2Context.IDContext,
        //      LOC_Strings2Context.ID AS IDString2Context,
        //      LOC_STRINGS.String,
        //      LOC_STRINGS.IDLanguage,
        //      LOC_STRINGS.IDType,
        //      LOC_StringTypes.Type AS StringType,
        //      LOC_STRINGS.ID AS IDString
        // FROM LOC_STRINGS
        // INNER JOIN LOC_Strings2Context ON LOC_STRINGS.ID = LOC_Strings2Context.IDString
        // INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
        // INNER JOIN LOC_CONTEXTS
        // INNER JOIN LOC_ConceptsTable
        // INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
        // INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context ON LOC_CONTEXTS.ID = LOC_Concept2Context.IDContext ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
        // WHERE (LOC_Job2Concept.IDJobList = @IDjob) AND
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace = @Internal) AND
        //       (@Internal IS NOT NULL) AND
        //       (LOC_ConceptsTable.Ignore = 0) AND
        //       (LOC_STRINGS.IDLanguage = @IDiso) OR
        //       (LOC_Job2Concept.IDJobList = @IDjob) AND
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace IS NULL) AND
        //       (@Internal IS  NULL) AND
        //       (LOC_ConceptsTable.Ignore = 0) AND
        //       (LOC_STRINGS.IDLanguage = @IDiso)
        public static IEnumerable<GroupledData> GetComplimentaryDataByComponentInternalISOjob(this LocalizationContext context, int idJobList, string ComponentName, string InternalNamespace, int iso)
        {
            string query =
                    $@"
                         SELECT
                              LOC_ConceptsTable.ID,
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace,
                              LOC_ConceptsTable.LocalizationID,
                              LOC_ConceptsTable.Comment,
                              LOC_Concept2Context.ID AS IDConcept2Context,
                              LOC_CONTEXTS.ContextName,
                              LOC_Concept2Context.IDContext,
                              LOC_Strings2Context.ID AS IDString2Context,
                              LOC_STRINGS.String,
                              LOC_STRINGS.IDLanguage,
                              LOC_STRINGS.IDType,
                              LOC_StringTypes.Type AS StringType,
                              LOC_STRINGS.ID AS IDString
                         FROM LOC_STRINGS
                         INNER JOIN LOC_Strings2Context ON LOC_STRINGS.ID = LOC_Strings2Context.IDString
                         INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
                         INNER JOIN LOC_CONTEXTS
                         INNER JOIN LOC_ConceptsTable
                         INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
                         INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context ON LOC_CONTEXTS.ID = LOC_Concept2Context.IDContext ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
                         WHERE (LOC_Job2Concept.IDJobList = {idJobList}) AND
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace = '{InternalNamespace}') AND
                               ('{InternalNamespace}' IS NOT NULL) AND
                               (LOC_ConceptsTable.Ignore = 0) AND
                               (LOC_STRINGS.IDLanguage = {iso}) OR
                               (LOC_Job2Concept.IDJobList = {idJobList}) AND
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace IS NULL) AND
                               ('{InternalNamespace}' IS  NULL) AND
                               (LOC_ConceptsTable.Ignore = 0) AND
                               (LOC_STRINGS.IDLanguage = {iso})
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<GroupledData> result = new List<GroupledData>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new GroupledData
                    {
                        ID = (int)reader[0],
                        ComponentNamespace = reader[1] as string,
                        InternalNamespace = reader[2] as string,
                        LocalizationID = reader[3] as string,
                        Comment = reader[4] as string,
                        IDConcept2Context = (int)reader[5],
                        ContextName = reader[6] as string,
                        IDContext = (int)reader[7],
                        IDString2Context = (int)reader[8],
                        String = reader[9] as string,
                        IDLanguage = (int)reader[10],
                        IDType = (int)reader[11],
                        StringType = reader[12] as string,
                        IDString = (int)reader[13]
                    });
                }
            }

            return result.ToList();
        }

        // SELECT
        //      LOC_Concept2Context.IDContext,
        //      LOC_ConceptsTable.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_Concept2Context.ID AS IDConcept2Context,
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_CONTEXTS.ContextName
        // FROM LOC_Concept2Context
        // INNER JOIN
        //      LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN
        //      LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // WHERE (LOC_ConceptsTable.ID = @ID)
        public static IEnumerable<Concept2ContextID> GetConcept2ContextIDsByConceptTableID(this LocalizationContext context, int conceptTableID)
        {
            var result = (from entity1 in context.LocConcept2Contexts
                          join entity2 in context.LocConceptsTables on entity1.Idconcept equals entity2.Id
                          join entity3 in context.LocContexts on entity1.Idconcept equals entity3.Id
                          where entity2.Id == conceptTableID
                          select new Concept2ContextID
                          {
                              IDContext = entity1.Idcontext,
                              IDConcept2Context = entity1.Id,
                              Id = entity2.Id,
                              ComponentNamespace = entity2.ComponentNamespace,
                              InternalNamespace = entity2.InternalNamespace,
                              LocalizationID = entity2.LocalizationId,
                              ContextName = entity3.ContextName
                          });

            return result.ToList();
        }


        // SELECT DISTINCT
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_Concept2Context.IDContext,
        //      LOC_ConceptsTable.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace, 
        //      LOC_Concept2Context.ID AS IDConcept2Context,
        //      LOC_CONTEXTS.ContextName
        // FROM LOC_Concept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // WHERE LOC_Concept2Context.ID= @IDConcept2Context
        public static IEnumerable<Concept2ContextID> GetDataByConcept2Context(this LocalizationContext context, int IDConcept2Context)
        {
            string query =
                    $@"
                         SELECT DISTINCT
                              LOC_ConceptsTable.LocalizationID,
                              LOC_Concept2Context.IDContext,
                              LOC_ConceptsTable.ID,
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace, 
                              LOC_Concept2Context.ID AS IDConcept2Context,
                              LOC_CONTEXTS.ContextName
                         FROM LOC_Concept2Context
                         INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
                         WHERE LOC_Concept2Context.ID= {IDConcept2Context}
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<Concept2ContextID> result = new List<Concept2ContextID>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Concept2ContextID
                    {
                        LocalizationID = reader[0] as string,
                        IDContext = (int)reader[1],
                        Id = (int)reader[2],
                        ComponentNamespace = reader[3] as string,
                        InternalNamespace = reader[4] as string,
                        IDConcept2Context = (int)reader[5],
                        ContextName = reader[6] as string
                    });
                }
            }

            return result.ToList();
        }

        // SELECT
        //      LOC_Concept2Context.IDContext,
        //      LOC_ConceptsTable.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_Concept2Context.ID AS IDConcept2Context,
        //      LOC_CONTEXTS.ContextName
        // FROM LOC_Concept2Context
        // INNER JOIN
        //      LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN
        //      LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        public static IEnumerable<Concept2ContextID> GetConceptAndContextData(this LocalizationContext context)
        {
            var result = (from entity1 in context.LocConcept2Contexts
                          join entity2 in context.LocConceptsTables on entity1.Idconcept equals entity2.Id
                          join entity3 in context.LocContexts on entity1.Idcontext equals entity3.Id
                          select new Concept2ContextID
                          {
                              IDContext = entity1.Idcontext,
                              Id = entity2.Id,
                              ComponentNamespace = entity2.ComponentNamespace,
                              InternalNamespace = entity2.InternalNamespace,
                              LocalizationID = entity2.LocalizationId,
                              IDConcept2Context = entity1.Id,
                              ContextName = entity3.ContextName
                          });

            return result.ToList();
        }
    }
}
