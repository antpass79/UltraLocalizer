using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class DataTableExtendedStringTableAdapter
    {
        // SELECT
        //      LOC_STRINGS_1.ID,
        //      LOC_STRINGS_1.IDLanguage,
        //      LOC_STRINGS_1.IDType,
        //      LOC_STRINGS_1.String,
        //      LOC_Languages.ISOCoding,
        //      LOC_Strings2Context.IDConcept2Context,
        //      (SELECT COUNT(*) AS isLocked
        // FROM LOC_STRINGSLocked
        // WHERE (LOC_STRINGS_1.ID = IDString)) AS isLocked,
        //      (SELECT COUNT(*) AS is2Translate
        //       FROM LOC_STRINGS2Translate
        //       WHERE(LOC_STRINGS_1.ID = IDString)) AS is2Translate,
        //      LOC_CONTEXTS.ContextName,
        //      LOC_StringTypes.Type,
        //      LOC_Concept2Context.IDContext, 
        //      LOC_Strings2Context.ID AS IDStrings2Context
        // FROM LOC_Strings2Context
        // INNER JOIN LOC_Concept2Context
        // INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
        // INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
        // INNER JOIN LOC_Languages ON LOC_STRINGS_1.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS_1.ID
        // INNER JOIN LOC_StringTypes ON LOC_STRINGS_1.IDType = LOC_StringTypes.ID
        // WHERE (LOC_Strings2Context.IDConcept2Context = @IDConcept2Context) AND (LOC_Languages.ISOCoding = @ISO)
        public static IEnumerable<DataTableExtendedStrings> GetStringByConcept2ContextISO(this LocalizationContext context, int Concept2Context, string ISO)
        {
            string query =
                    $@"
                         SELECT
                              LOC_STRINGS_1.ID,
                              LOC_STRINGS_1.IDLanguage,
                              LOC_STRINGS_1.IDType,
                              LOC_STRINGS_1.String,
                              LOC_Languages.ISOCoding,
                              LOC_Strings2Context.IDConcept2Context,
                              (SELECT COUNT(*) AS isLocked
                         FROM LOC_STRINGSLocked
                         WHERE (LOC_STRINGS_1.ID = IDString)) AS isLocked,
                              (SELECT COUNT(*) AS is2Translate
                               FROM LOC_STRINGS2Translate
                               WHERE(LOC_STRINGS_1.ID = IDString)) AS is2Translate,
                              LOC_CONTEXTS.ContextName,
                              LOC_StringTypes.Type,
                              LOC_Concept2Context.IDContext, 
                              LOC_Strings2Context.ID AS IDStrings2Context
                         FROM LOC_Strings2Context
                         INNER JOIN LOC_Concept2Context
                         INNER JOIN LOC_CONTEXTS ON LOC_Concept2Context.IDContext = LOC_CONTEXTS.ID ON LOC_Strings2Context.IDConcept2Context = LOC_Concept2Context.ID
                         INNER JOIN LOC_STRINGS AS LOC_STRINGS_1
                         INNER JOIN LOC_Languages ON LOC_STRINGS_1.IDLanguage = LOC_Languages.ID ON LOC_Strings2Context.IDString = LOC_STRINGS_1.ID
                         INNER JOIN LOC_StringTypes ON LOC_STRINGS_1.IDType = LOC_StringTypes.ID
                         WHERE (LOC_Strings2Context.IDConcept2Context = '{Concept2Context}') AND (LOC_Languages.ISOCoding = '{ISO}')
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<DataTableExtendedStrings> result = new List<DataTableExtendedStrings>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DataTableExtendedStrings
                    {
                        ID = (int)reader[0],
                        IDLanguage = (int)reader[1],
                        IDType = (int)reader[2],
                        String = reader[3] as string,
                        ISOCoding = reader[4] as string,
                        IDConcept2Context = (int)reader[5],
                        IsLocked = (int)reader[6] != 0,
                        Is2Translate = (int)reader[7] != 7,
                        ContextName = reader[8] as string,
                        Type = reader[9] as string,
                        IDContext = (int)reader[10],
                        IDString2Context = (int)reader[11]
                    });
                }
            }

            return result.ToList();
        }
    }
}
