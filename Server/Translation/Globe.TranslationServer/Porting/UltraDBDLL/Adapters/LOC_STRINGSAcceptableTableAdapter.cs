using Globe.TranslationServer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class LOC_STRINGSAcceptableTableAdapter
    {
        //DELETE FROM[LOC_STRINGSAcceptable] WHERE([ID_String] = @Original_ID)
        public static void DeleteAcceptable(this LocalizationContext context, int IDString)
        {
            // ANTO check id and idstring keys
            var stringToRemove = context.LocStringsacceptables.Find(IDString);
            context.LocStringsacceptables.Remove(stringToRemove);

            context.SaveChanges();
        }

        //INSERT INTO[LOC_STRINGSAcceptable] ([ID_String]) VALUES(@ID_String);
        //SELECT ID, ID_String FROM LOC_STRINGSAcceptable WHERE(ID = SCOPE_IDENTITY())
        public static int InsertNewAcceptable(this LocalizationContext context, int IDString)
        {
            context.LocStringsacceptables.Add(new LocStringsacceptable
            {
                IdString = IDString
            });

            return context.SaveChanges();
        }


        //            SELECT CAST(COUNT(ID) AS bit) AS isAcceptable
        //FROM LOC_STRINGSAcceptable
        //WHERE(ID_String = @LOC_STRINGSID)
        public static bool isAcceptable(this LocalizationContext context, int IDString)
        {
            string query =
                    $@"
                                    SELECT CAST(COUNT(ID) AS bit) AS isAcceptable
                        FROM LOC_STRINGSAcceptable
                        WHERE(ID_String = {IDString})
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            bool isAcceptable = false;


            using (SqlDataReader reader = command.ExecuteReader())
            {
                isAcceptable = reader.HasRows;
            }

            return isAcceptable;
        }
    }
}
