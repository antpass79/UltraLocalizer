﻿using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class STRINGSTableAdapter
    {
        // SELECT
        //      LOC_STRINGS.ID,
        //      LOC_STRINGS.IDLanguage,
        //      LOC_STRINGS.IDType,
        //      LOC_STRINGS.String,
        //      LOC_Strings2Context.ID AS IDString2Context
        // FROM            LOC_STRINGS INNER JOIN
        //                         LOC_Strings2Context ON LOC_STRINGS.ID = LOC_Strings2Context.IDString
        // WHERE        (LOC_Strings2Context.IDConcept2Context = @IDConcept2Context)
        public static IEnumerable<STRING> GetDataByConcept2ContextStrings(this LocalizationContext context, int IDConcept2Context)
        {
            string query =
                    $@"
                         SELECT
                              LOC_STRINGS.ID,
                              LOC_STRINGS.IDLanguage,
                              LOC_STRINGS.IDType,
                              LOC_STRINGS.String,
                              LOC_Strings2Context.ID AS IDString2Context
                         FROM            LOC_STRINGS INNER JOIN
                                                 LOC_Strings2Context ON LOC_STRINGS.ID = LOC_Strings2Context.IDString
                         WHERE        (LOC_Strings2Context.IDConcept2Context = '{IDConcept2Context}')
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<STRING> result = new List<STRING>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new STRING
                    {
                        ID = (int)reader[0],
                        IDLanguage = (int)reader[1],
                        IDType = (int)reader[2],
                        String = reader[3] as string,
                        IDString2Context = (int)reader[4],
                    });
                }
            }

            return result.ToList();
        }

        //        SELECT LOC_STRINGS.ID, LOC_STRINGS.IDLanguage, LOC_STRINGS.IDType, LOC_STRINGS.String, LOC_Strings2Context.ID AS IDString2Context
        //FROM            LOC_STRINGS INNER JOIN
        //                         LOC_Strings2Context ON LOC_STRINGS.ID = LOC_Strings2Context.IDString
        //WHERE        (LOC_Strings2Context.IDConcept2Context =
        //                             (SELECT        TOP (1) LOC_Strings2Context_1.IDConcept2Context AS Expr1
        //                               FROM            LOC_STRINGS AS LOC_STRINGS_1 INNER JOIN
        //                                                         LOC_Strings2Context AS LOC_Strings2Context_1 ON LOC_STRINGS_1.ID = LOC_Strings2Context_1.IDString
        //                               WHERE        (LOC_Strings2Context_1.IDString = @IDstr)))
        public static IEnumerable<STRING> GetConceptContextEquivalentStrings(this LocalizationContext context, int IDString)
        {
            string query =
                    $@"
                            SELECT LOC_STRINGS.ID, LOC_STRINGS.IDLanguage, LOC_STRINGS.IDType, LOC_STRINGS.String, LOC_Strings2Context.ID AS IDString2Context
                    FROM            LOC_STRINGS INNER JOIN
                                             LOC_Strings2Context ON LOC_STRINGS.ID = LOC_Strings2Context.IDString
                    WHERE        (LOC_Strings2Context.IDConcept2Context =
                                                 (SELECT        TOP (1) LOC_Strings2Context_1.IDConcept2Context AS Expr1
                                                   FROM            LOC_STRINGS AS LOC_STRINGS_1 INNER JOIN
                                                                             LOC_Strings2Context AS LOC_Strings2Context_1 ON LOC_STRINGS_1.ID = LOC_Strings2Context_1.IDString
                                                   WHERE        (LOC_Strings2Context_1.IDString = '{IDString}')))
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<STRING> result = new List<STRING>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new STRING
                    {
                        ID = (int)reader[0],
                        IDLanguage = (int)reader[1],
                        IDType = (int)reader[2],
                        String = reader[3] as string,
                        IDString2Context = (int)reader[4],
                    });
                }
            }

            return result.ToList();
        }

        //        SELECT LOC_STRINGS.*
        //FROM LOC_STRINGS
        //where ID = @IDString
        public static IEnumerable<STRING> GetStringByID(this LocalizationContext context, int ID)
        {
            IList<STRING> strings = new List<STRING>();
            var item = context.LocStrings.Find(ID);
            strings.Add(new STRING
            {
                ID = item.Id,
                IDLanguage = item.Idlanguage,
                IDType = item.Idtype,
                String = item.String
            });

            return strings;

            string query =
                    $@"
                            SELECT LOC_STRINGS.*
                    FROM LOC_STRINGS
                    where ID = '{ID}'
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<STRING> result = new List<STRING>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new STRING
                    {
                        ID = (int)reader[0],
                        IDLanguage = (int)reader[1],
                        IDType = (int)reader[2],
                        String = reader[3] as string
                    });
                }
            }

            return result.ToList();
        }

        //        UPDATE LOC_STRINGS SET String = @String WHERE ID = @ID
        //SELECT String FROM LOC_STRINGS WHERE(ID = @ID)
        public static void UpdatebyID(this LocalizationContext context, string DataString, int ID)
        {
            var stringToUpdate = context.LocStrings.Find(ID);
            stringToUpdate.String = DataString;
            //context.SaveChanges();
        }

        //        INSERT INTO[LOC_STRINGS] ([IDLanguage], [IDType], [String]) VALUES(@IDLanguage, @IDType, @String);
        //        SELECT ID, IDLanguage, IDType, String FROM LOC_STRINGS WHERE(ID = SCOPE_IDENTITY())
        //SELECT @@IDENTITY
        public static int InsertNewString(this LocalizationContext context, int IDLanguage, int IDType, string DataString)
        {
            throw new NotImplementedException();

            context.LocStrings.Add(new LocStrings
            {
                Idlanguage = IDLanguage,
                Idtype = IDType,
                String = DataString
            });
            //return context.SaveChanges();
        }
    }
}