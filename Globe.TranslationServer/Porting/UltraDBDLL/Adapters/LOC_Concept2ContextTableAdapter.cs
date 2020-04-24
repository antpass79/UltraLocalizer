using Globe.TranslationServer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class LOC_Concept2ContextTableAdapter
    {
        // SELECT ID
        // FROM LOC_Concept2Context
        public static IEnumerable<int> GetAllC2CData(this LocalizationContext context)
        {
            var result = from entity in context.LocConcept2Context
                         select entity.Id;

            return result.ToList();
        }

        // SELECT DISTINCT LOC_Concept2Context.ID
        // FROM LOC_Concept2Context
        // INNER JOIN LOC_Strings2Context ON LOC_Concept2Context.ID = LOC_Strings2Context.IDConcept2Context
        // INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
        // WHERE (LOC_Strings2Context.IDString = @StringID) AND (LOC_ConceptsTable.Ignore = 0) and LOC_Concept2Context.ID
        // NOT IN
        //      (SELECT LOC_Concept2Context_1.ID
        //       FROM LOC_Strings2Context AS LOC_Strings2Context_1
        //       INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1 ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
        //       INNER JOIN LOC_STRINGS AS LOC_STRINGS_1 ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
        //       WHERE (LOC_STRINGS_1.IDLanguage = @ISO))
        public static IEnumerable<int> GetSiblingsByIDStringISO(this LocalizationContext context, int stringID, int isocoding)
        {
            string query =
                    $@"
                         SELECT DISTINCT LOC_Concept2Context.ID
                         FROM LOC_Concept2Context
                         INNER JOIN LOC_Strings2Context ON LOC_Concept2Context.ID = LOC_Strings2Context.IDConcept2Context
                         INNER JOIN LOC_ConceptsTable ON LOC_Concept2Context.IDConcept = LOC_ConceptsTable.ID
                         WHERE (LOC_Strings2Context.IDString = '{stringID}') AND (LOC_ConceptsTable.Ignore = 0) and LOC_Concept2Context.ID
                         NOT IN
                              (SELECT LOC_Concept2Context_1.ID
                               FROM LOC_Strings2Context AS LOC_Strings2Context_1
                               INNER JOIN LOC_Concept2Context AS LOC_Concept2Context_1 ON LOC_Strings2Context_1.IDConcept2Context = LOC_Concept2Context_1.ID
                               INNER JOIN LOC_STRINGS AS LOC_STRINGS_1 ON LOC_Strings2Context_1.IDString = LOC_STRINGS_1.ID
                               WHERE (LOC_STRINGS_1.IDLanguage = '{isocoding}'))
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<int> result = new List<int>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add((int)reader[0]);
                }
            }

            return result.ToList();
        }
    }
}
