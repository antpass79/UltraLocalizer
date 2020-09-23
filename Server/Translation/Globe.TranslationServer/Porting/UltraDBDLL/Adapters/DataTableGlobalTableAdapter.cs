using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class DataTableGlobalTableAdapter
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
        // RIGHT OUTER JOIN LOC_CONTEXTS
        // INNER JOIN LOC_ConceptsTable
        // INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
        // INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context ON LOC_CONTEXTS.ID = LOC_Concept2Context.IDContext ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
        // WHERE (LOC_Job2Concept.IDJobList = @IDjob) AND
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace = @Internal) AND
        //       (@Internal IS NOT NULL) AND
        //       (LOC_Strings2Context.ID IS NULL) OR
        //       (LOC_Job2Concept.IDJobList = @IDjob) AND
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace = @Internal) AND
        //       (@Internal IS NOT NULL) AND
        //       (LOC_Strings2Context.ID IS NOT NULL) AND
        //       (LOC_STRINGS.IDLanguage = 1) OR
        //       (LOC_Job2Concept.IDJobList = @IDjob) AND
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace IS NULL) AND
        //       (@Internal IS NULL) AND(LOC_Strings2Context.ID IS NULL) OR
        //       (LOC_Job2Concept.IDJobList = @IDjob) AND
        //       (LOC_ConceptsTable.ComponentNamespace = @Component) AND
        //       (LOC_ConceptsTable.InternalNamespace IS NULL) AND
        //       (@Internal IS NULL) AND(LOC_Strings2Context.ID IS NOT NULL) AND(LOC_STRINGS.IDLanguage = 1)
        public static IEnumerable<GroupledData> GetEngDatabyComponentInternal(this LocalizationContext context, int idJobList, string ComponentName, string InternalNamespace)
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
                         RIGHT OUTER JOIN LOC_CONTEXTS
                         INNER JOIN LOC_ConceptsTable
                         INNER JOIN LOC_Concept2Context ON LOC_ConceptsTable.ID = LOC_Concept2Context.IDConcept
                         INNER JOIN LOC_Job2Concept ON LOC_Concept2Context.ID = LOC_Job2Concept.IDConcept2Context ON LOC_CONTEXTS.ID = LOC_Concept2Context.IDContext ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
                         WHERE (LOC_Job2Concept.IDJobList = '{idJobList}') AND
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace = '{InternalNamespace}') AND
                               ('{InternalNamespace}' IS NOT NULL) AND
                               (LOC_Strings2Context.ID IS NULL) OR
                               (LOC_Job2Concept.IDJobList = '{idJobList}') AND
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace = '{InternalNamespace}') AND
                               ('{InternalNamespace}' IS NOT NULL) AND
                               (LOC_Strings2Context.ID IS NOT NULL) AND
                               (LOC_STRINGS.IDLanguage = 1) OR
                               (LOC_Job2Concept.IDJobList = '{idJobList}') AND
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace IS NULL) AND
                               ('{InternalNamespace}' IS NULL) AND(LOC_Strings2Context.ID IS NULL) OR
                               (LOC_Job2Concept.IDJobList = '{idJobList}') AND
                               (LOC_ConceptsTable.ComponentNamespace = '{ComponentName}') AND
                               (LOC_ConceptsTable.InternalNamespace IS NULL) AND
                               ('{InternalNamespace}' IS NULL) AND(LOC_Strings2Context.ID IS NOT NULL) AND(LOC_STRINGS.IDLanguage = 1)
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<GroupledData> result = new List<GroupledData>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    try 
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
                            IDString2Context = !string.IsNullOrWhiteSpace(reader[8].ToString()) ? (int)reader[8] : 0,
                            String = reader[9] as string,
                            IDLanguage = !string.IsNullOrWhiteSpace(reader[10].ToString()) ? (int)reader[10] : 0,
                            IDType = !string.IsNullOrWhiteSpace(reader[11].ToString()) ? (int)reader[11] : 0,
                            StringType = reader[12] as string,
                            IDString = !string.IsNullOrWhiteSpace(reader[13].ToString()) ? (int)reader[13] : 0
                        });
                    }
                    catch(Exception e)
                    {
                        

                    }
                }
            }

            return result.ToList();
        }

        // SELECT DISTINCT
        //      LOC_Concept2Context.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID, 
        //      LOC_Languages.ISOCoding,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_STRINGS.String,
        //      LOC_STRINGS.ID as StringID,
        //      LOC_STRINGS.IDType, 
        //      LOC_StringTypes.Type,
        //      LOC_ConceptsTable.Ignore,
        //      LOC_ConceptsTable.ID as ConceptID
        // FROM LOC_Strings2Context
        // INNER JOIN LOC_Concept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
        // INNER JOIN LOC_STRINGS
        // INNER JOIN LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
        // INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
        // INNER JOIN LOC_StringTypes AS LOC_StringTypes_1 ON LOC_STRINGS.IDType = LOC_StringTypes_1.ID
        // WHERE (LOC_ConceptsTable.Ignore = 0) AND (LOC_ConceptsTable.ComponentNamespace = @Component) AND (LOC_Languages.ISOCoding = 'en')
        //      AND (LOC_Concept2Context.ID NOT IN
        //          (SELECT LOC_Concept2Context_1.ID
        //           FROM LOC_Strings2Context AS LOC_Strings2Context_1
        //           INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1
        //           INNER JOIN LOC_ConceptsTable AS LOC_ConceptsTable_1 ON LOC_Concept2Context_1.IDConcept = LOC_ConceptsTable_1.ID
        //           INNER JOIN LOC_CONTEXTS AS LOC_CONTEXTS_1 ON LOC_Concept2Context_1.IDContext = LOC_CONTEXTS_1.ID ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
        //           INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
        //           INNER JOIN LOC_Languages AS LOC_Languages_1 ON LOC_STRINGS_1.IDLanguage = LOC_Languages_1.ID ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
        //           WHERE (LOC_ConceptsTable_1.ComponentNamespace = @Component) AND (LOC_Languages_1.ISOCoding = @ISO) AND (LOC_ConceptsTable_1.InternalNamespace = @Internal))) AND (LOC_ConceptsTable.InternalNamespace = @Internal)
        public static IEnumerable<DataTableGlobal> GetMissingDataByComponentISOInternal(this LocalizationContext context, string componentName, string internalNamespace, string isocoding)
        {
            string query =
                    $@"
                         SELECT DISTINCT
                              LOC_Concept2Context.ID,
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace,
                              LOC_ConceptsTable.LocalizationID, 
                              LOC_Languages.ISOCoding,
                              LOC_CONTEXTS.ContextName,
                              LOC_STRINGS.String,
                              LOC_STRINGS.ID as StringID,
                              LOC_STRINGS.IDType, 
                              LOC_StringTypes.Type,
                              LOC_ConceptsTable.Ignore,
                              LOC_ConceptsTable.ID as ConceptID
                         FROM LOC_Strings2Context
                         INNER JOIN LOC_Concept2Context
                         INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
                         INNER JOIN LOC_STRINGS
                         INNER JOIN LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
                         INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
                         INNER JOIN LOC_StringTypes AS LOC_StringTypes_1 ON LOC_STRINGS.IDType = LOC_StringTypes_1.ID
                         WHERE (LOC_ConceptsTable.Ignore = 0) AND (LOC_ConceptsTable.ComponentNamespace = '{componentName}') AND (LOC_Languages.ISOCoding = 'en')
                              AND (LOC_Concept2Context.ID NOT IN
                                  (SELECT LOC_Concept2Context_1.ID
                                   FROM LOC_Strings2Context AS LOC_Strings2Context_1
                                   INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1
                                   INNER JOIN LOC_ConceptsTable AS LOC_ConceptsTable_1 ON LOC_Concept2Context_1.IDConcept = LOC_ConceptsTable_1.ID
                                   INNER JOIN LOC_CONTEXTS AS LOC_CONTEXTS_1 ON LOC_Concept2Context_1.IDContext = LOC_CONTEXTS_1.ID ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
                                   INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
                                   INNER JOIN LOC_Languages AS LOC_Languages_1 ON LOC_STRINGS_1.IDLanguage = LOC_Languages_1.ID ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
                                   WHERE (LOC_ConceptsTable_1.ComponentNamespace = '{componentName}') AND (LOC_Languages_1.ISOCoding = '{isocoding}') AND (LOC_ConceptsTable_1.InternalNamespace = '{internalNamespace}'))) AND (LOC_ConceptsTable.InternalNamespace = '{internalNamespace}')
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableGlobal> result = new List<DataTableGlobal>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableGlobal
                    {
                        ID = (int)reader[0],
                        ComponentNamespace = reader[1] as string,
                        InternalNamespace = reader[2] as string,
                        LocalizationID = reader[3] as string,
                        ISOCoding = isocoding,
                        ContextName = reader[5] as string,
                        String = reader[6] as string,
                        IDType = (int)reader[7],
                        Type = reader[8] as string,
                        StringID = (int)reader[9],
                        Ignore = (bool)reader[10],
                        ConceptID = (int)reader[11],
                        Expr1 = (int)reader[12],
                        IsAcceptable = (bool)reader[13]
                    });
                }
            }

            return result.ToList();
        }

        // SELECT DISTINCT
        //      LOC_Concept2Context.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID, 
        //      LOC_Languages.ISOCoding,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_STRINGS.String,
        //      LOC_STRINGS.ID as StringID,
        //      LOC_STRINGS.IDType, 
        //      LOC_StringTypes.Type,
        //      LOC_ConceptsTable.Ignore,
        //      LOC_ConceptsTable.ID as ConceptID
        // FROM LOC_Strings2Context
        // INNER JOIN LOC_Concept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
        // INNER JOIN LOC_STRINGS
        // INNER JOIN LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
        // INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
        // WHERE (LOC_ConceptsTable.Ignore = 0) AND (LOC_ConceptsTable.ComponentNamespace = @Component) AND (LOC_Languages.ISOCoding = 'en') AND (LOC_Concept2Context.ID
        // NOT IN 
        //      (SELECT LOC_Concept2Context_1.ID
        //      FROM LOC_Strings2Context AS LOC_Strings2Context_1
        //      INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1
        //      INNER JOIN LOC_ConceptsTable AS LOC_ConceptsTable_1 ON LOC_Concept2Context_1.IDConcept = LOC_ConceptsTable_1.ID
        //      INNER JOIN LOC_CONTEXTS AS LOC_CONTEXTS_1 ON LOC_Concept2Context_1.IDContext = LOC_CONTEXTS_1.ID ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
        //      INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
        //      INNER JOIN LOC_Languages AS LOC_Languages_1 ON LOC_STRINGS_1.IDLanguage = LOC_Languages_1.ID ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
        //      WHERE (LOC_ConceptsTable_1.ComponentNamespace = @Component) AND (LOC_Languages_1.ISOCoding = @ISO) AND (LOC_ConceptsTable_1.InternalNamespace IS NULL))) AND (LOC_ConceptsTable.InternalNamespace IS NULL)
        public static IEnumerable<DataTableGlobal> GetMissingDataByComponentISO(this LocalizationContext context, string componentName, string isocoding)
        {
            string query =
                    $@"
                         SELECT DISTINCT
                              LOC_Concept2Context.ID,
                              LOC_ConceptsTable.ComponentNamespace,
                              LOC_ConceptsTable.InternalNamespace,
                              LOC_ConceptsTable.LocalizationID, 
                              LOC_Languages.ISOCoding,
                              LOC_CONTEXTS.ContextName,
                              LOC_STRINGS.String,
                              LOC_STRINGS.ID as StringID,
                              LOC_STRINGS.IDType, 
                              LOC_StringTypes.Type,
                              LOC_ConceptsTable.Ignore,
                              LOC_ConceptsTable.ID as ConceptID
                         FROM LOC_Strings2Context
                         INNER JOIN LOC_Concept2Context
                         INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
                         INNER JOIN LOC_STRINGS
                         INNER JOIN LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
                         INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
                         WHERE (LOC_ConceptsTable.Ignore = 0) AND (LOC_ConceptsTable.ComponentNamespace = '{componentName}') AND (LOC_Languages.ISOCoding = 'en') AND (LOC_Concept2Context.ID
                         NOT IN 
                              (SELECT LOC_Concept2Context_1.ID
                              FROM LOC_Strings2Context AS LOC_Strings2Context_1
                              INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1
                              INNER JOIN LOC_ConceptsTable AS LOC_ConceptsTable_1 ON LOC_Concept2Context_1.IDConcept = LOC_ConceptsTable_1.ID
                              INNER JOIN LOC_CONTEXTS AS LOC_CONTEXTS_1 ON LOC_Concept2Context_1.IDContext = LOC_CONTEXTS_1.ID ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
                              INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
                              INNER JOIN LOC_Languages AS LOC_Languages_1 ON LOC_STRINGS_1.IDLanguage = LOC_Languages_1.ID ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
                              WHERE (LOC_ConceptsTable_1.ComponentNamespace = '{componentName}') AND (LOC_Languages_1.ISOCoding = '{isocoding}') AND (LOC_ConceptsTable_1.InternalNamespace IS NULL))) AND (LOC_ConceptsTable.InternalNamespace IS NULL)
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableGlobal> result = new List<DataTableGlobal>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableGlobal
                    {
                        ID = (int)reader[0],
                        ComponentNamespace = reader[1] as string,
                        InternalNamespace = reader[2] as string,
                        LocalizationID = reader[3] as string,
                        ISOCoding = isocoding,
                        ContextName = reader[5] as string,
                        String = reader[6] as string,
                        IDType = (int)reader[7],
                        Type = reader[8] as string,
                        StringID = (int)reader[9],
                        Ignore = (bool)reader[10],
                        ConceptID = (int)reader[11],
                        Expr1 = (int)reader[12],
                        IsAcceptable = (bool)reader[13]
                    });
                }
            }

            return result.ToList();
        }

        // SELECT DISTINCT
        //      LOC_Concept2Context.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID,
        //      LOC_Languages.ISOCoding, 
        //      LOC_CONTEXTS.ContextName,
        //      LOC_STRINGS.String,
        //      LOC_STRINGS.ID as StringID,
        //      LOC_STRINGS.IDType,
        //      LOC_StringTypes.Type,
        //      LOC_ConceptsTable.Ignore,
        //      LOC_ConceptsTable.ID as ConceptID
        // FROM LOC_Strings2Context
        // INNER JOIN LOC_Concept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
        // INNER JOIN LOC_STRINGS
        // INNER JOIN LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
        // INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
        // WHERE (LOC_ConceptsTable.Ignore = 0) AND (LOC_Languages.ISOCoding = 'en') AND (LOC_Concept2Context.ID
        // NOT IN (SELECT LOC_Concept2Context_1.ID
        //          FROM LOC_Strings2Context AS LOC_Strings2Context_1
        //          INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1
        //          INNER JOIN LOC_ConceptsTable AS LOC_ConceptsTable_1 ON LOC_Concept2Context_1.IDConcept = LOC_ConceptsTable_1.ID
        //          INNER JOIN LOC_CONTEXTS AS LOC_CONTEXTS_1 ON LOC_Concept2Context_1.IDContext = LOC_CONTEXTS_1.ID ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
        //          INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
        //          INNER JOIN LOC_Languages AS LOC_Languages_1 ON LOC_STRINGS_1.IDLanguage = LOC_Languages_1.ID ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
        //          WHERE (LOC_ConceptsTable_1.ID = @ID) AND (LOC_Languages_1.ISOCoding = @ISO))) AND (LOC_ConceptsTable.ID = @ID)
        public static IEnumerable<DataTableGlobal> GetMissingDataByConceptID(this LocalizationContext context, int ConceptID, string isocoding)
        {
            string query =
                    $@"
                        SELECT DISTINCT
                                LOC_Concept2Context.ID,
                                LOC_ConceptsTable.ComponentNamespace,
                                LOC_ConceptsTable.InternalNamespace,
                                LOC_ConceptsTable.LocalizationID,
                                LOC_Languages.ISOCoding, 
                                LOC_CONTEXTS.ContextName,
                                LOC_STRINGS.String,
                                LOC_STRINGS.ID as StringID,
                                LOC_STRINGS.IDType,
                                LOC_StringTypes.Type,
                                LOC_ConceptsTable.Ignore,
                                LOC_ConceptsTable.ID as ConceptID
                            FROM LOC_Strings2Context
                            INNER JOIN LOC_Concept2Context
                            INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                            INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
                            INNER JOIN LOC_STRINGS
                            INNER JOIN LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
                            INNER JOIN LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
                            WHERE (LOC_ConceptsTable.Ignore = 0) AND (LOC_Languages.ISOCoding = 'en') AND (LOC_Concept2Context.ID
                            NOT IN (SELECT LOC_Concept2Context_1.ID
                                    FROM LOC_Strings2Context AS LOC_Strings2Context_1
                                    INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1
                                    INNER JOIN LOC_ConceptsTable AS LOC_ConceptsTable_1 ON LOC_Concept2Context_1.IDConcept = LOC_ConceptsTable_1.ID
                                    INNER JOIN LOC_CONTEXTS AS LOC_CONTEXTS_1 ON LOC_Concept2Context_1.IDContext = LOC_CONTEXTS_1.ID ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
                                    INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
                                    INNER JOIN LOC_Languages AS LOC_Languages_1 ON LOC_STRINGS_1.IDLanguage = LOC_Languages_1.ID ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
                                    WHERE (LOC_ConceptsTable_1.ID = '{ConceptID}') AND (LOC_Languages_1.ISOCoding = '{isocoding}'))) AND (LOC_ConceptsTable.ID = '{ConceptID}')
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableGlobal> result = new List<DataTableGlobal>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableGlobal
                    {
                        ID = (int)reader[0],
                        ComponentNamespace = reader[1] as string,
                        InternalNamespace = reader[2] as string,
                        LocalizationID = reader[3] as string,
                        ISOCoding = isocoding,
                        ContextName = reader[5] as string,
                        String = reader[6] as string,
                        IDType = (int)reader[7],
                        Type = reader[8] as string,
                        StringID = (int)reader[9],
                        Ignore = (bool)reader[10],
                        ConceptID = (int)reader[11],
                        Expr1 = (int)reader[12],
                        IsAcceptable = (bool)reader[13]
                    });
                }
            }

            return result.ToList();
        }

        // SELECT DISTINCT
        //      LOC_Concept2Context.ID,
        //      LOC_ConceptsTable.ComponentNamespace,
        //      LOC_ConceptsTable.InternalNamespace,
        //      LOC_ConceptsTable.LocalizationID, 
        //      LOC_Languages.ISOCoding,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_STRINGS.String,
        //      LOC_STRINGS.IDType,
        //      LOC_StringTypes.Type,
        //      LOC_STRINGS.ID AS StringID, 
        //      LOC_ConceptsTable.Ignore,
        //      LOC_ConceptsTable.ID AS ConceptID,
        // (SELECT CAST(COUNT(ID) AS bit) AS Expr1
        // FROM LOC_STRINGSAcceptable
        // WHERE(ID_String = LOC_STRINGS.ID)) AS isAcceptable
        // FROM LOC_ConceptsTable INNER JOIN
        //      LOC_Concept2Context ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // INNER JOIN
        //      LOC_Strings2Context ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
        // INNER JOIN
        //      LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
        // INNER JOIN
        //      LOC_STRINGS
        // INNER JOIN
        //      LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
        // INNER JOIN
        //      LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
        // WHERE (LOC_ConceptsTable.ComponentNamespace = @Component)
        //      AND (LOC_Languages.ISOCoding = @ISO)
        //      AND (LOC_ConceptsTable.Ignore = 0)
        //      OR (LOC_ConceptsTable.ComponentNamespace = @Component)
        //      AND (LOC_Languages.ISOCoding = 'en')
        //      AND (LOC_ConceptsTable.Ignore = 1)
        public static IEnumerable<DataTableGlobal> GetDatabyComponentISO(this LocalizationContext context, string componentName, string isoCoding)
        {
            string query =
                    $@"SELECT DISTINCT
                          LOC_Concept2Context.ID,
                          LOC_ConceptsTable.ComponentNamespace,
                          LOC_ConceptsTable.InternalNamespace,
                          LOC_ConceptsTable.LocalizationID, 
                          LOC_Languages.ISOCoding,
                          LOC_CONTEXTS.ContextName,
                          LOC_STRINGS.String,
                          LOC_STRINGS.IDType,
                          LOC_StringTypes.Type,
                          LOC_STRINGS.ID AS StringID, 
                          LOC_ConceptsTable.Ignore,
                          LOC_ConceptsTable.ID AS ConceptID,
                     (SELECT CAST(COUNT(ID) AS bit) AS Expr1
                     FROM LOC_STRINGSAcceptable
                     WHERE(ID_String = LOC_STRINGS.ID)) AS isAcceptable
                     FROM LOC_ConceptsTable INNER JOIN
                          LOC_Concept2Context ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                     INNER JOIN
                          LOC_Strings2Context ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
                     INNER JOIN
                          LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID
                     INNER JOIN
                          LOC_STRINGS
                     INNER JOIN
                          LOC_Languages ON LOC_STRINGS.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS.ID
                     INNER JOIN
                          LOC_StringTypes ON LOC_STRINGS.IDType = LOC_StringTypes.ID
                     WHERE(LOC_ConceptsTable.ComponentNamespace = '{componentName}')
                          AND(LOC_Languages.ISOCoding = '{isoCoding}')
                          AND(LOC_ConceptsTable.Ignore = 0)
                          OR(LOC_ConceptsTable.ComponentNamespace = '{componentName}')
                          AND(LOC_Languages.ISOCoding = 'en')
                          AND(LOC_ConceptsTable.Ignore = 1)";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableGlobal> result = new List<DataTableGlobal>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableGlobal
                    {
                        ID = (int)reader[0],
                        ComponentNamespace = reader[1] as string,
                        InternalNamespace = reader[2] as string,
                        LocalizationID = reader[3] as string,
                        ISOCoding = reader[4] as string,
                        ContextName = reader[5] as string,
                        String = reader[6] as string,
                        IDType = (int)reader[7],
                        Type = reader[8] as string,
                        StringID = (int)reader[9],
                        Ignore = (bool)reader[10],
                        ConceptID = (int)reader[11],
                        IsAcceptable = (bool)reader[12],
                    });
                }
            }

            return result.ToList();
        }
    }
}
