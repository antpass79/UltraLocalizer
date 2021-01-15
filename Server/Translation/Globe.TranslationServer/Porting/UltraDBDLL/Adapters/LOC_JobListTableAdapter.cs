using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.Adapters
{
    public static class LOC_JobListTableAdapter
    {
        //        SELECT ID, JobName, UserName, IDIsoCoding
        //FROM LOC_JobList
        //WHERE(IDIsoCoding = @IDiso)
        public static IEnumerable<LOC_JobList> GetDataByIDIso(this LocalizationContext context, int idIso)
        {
            string query =
                $@"
                        SELECT ID, JobName, UserName, IDIsoCoding
                FROM LOC_JobList
                WHERE(IDIsoCoding = '{idIso}')
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<LOC_JobList> result = new List<LOC_JobList>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new LOC_JobList
                    {
                        ID = (int)reader[0],
                        JobName = reader[1] as string,
                        UserName = reader[2] as string,
                        IDIsoCoding = (int)reader[3],
                    });
                }
            }

            return result.ToList();
        }

        //SELECT ID, JobName, UserName, IDIsoCoding FROM dbo.LOC_JobList where UserName = @UserName AND IDIsoCoding = @idIsoCoding
        public static IEnumerable<LOC_JobList> GetDataByUserISO(this LocalizationContext context, string userName, int idIsoCoding)
        {
            string query =
                $@"
                    SELECT ID, JobName, UserName, IDIsoCoding FROM dbo.LOC_JobList where UserName='{userName}' AND IDIsoCoding='{idIsoCoding}'
                ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<LOC_JobList> result = new List<LOC_JobList>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new LOC_JobList
                    {
                        ID = (int)reader[0],
                        JobName = reader[1] as string,
                        UserName = reader[2] as string,
                        IDIsoCoding = (int)reader[3],
                    });
                }
            }

            return result.ToList();
        }


        //        SELECT ID, JobName, UserName, IDIsoCoding
        //FROM LOC_JobList
        //WHERE(UserName = @Username) AND(IDIsoCoding = @IDiso)
        public static IEnumerable<LOC_JobList> GetDataByUserNameIDISO(this LocalizationContext context, string UserName, int idIso)
        {
            string query =
                $@"
                            SELECT ID, JobName, UserName, IDIsoCoding
                    FROM LOC_JobList
                    WHERE(UserName = '{UserName}') AND(IDIsoCoding = '{idIso}')
                    ";

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();

            List<LOC_JobList> result = new List<LOC_JobList>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new LOC_JobList
                    {
                        ID = (int)reader[0],
                        JobName = reader[1] as string,
                        UserName = reader[2] as string,
                        IDIsoCoding = (int)reader[3],
                    });
                }
            }

            return result.ToList();
        }

        public static int Delete(this LocalizationContext context, int Original_ID, string Original_JobName, string Original_UserName, int Original_IDIsoCoding)
        {
            // ANTO not found in the original code
            throw new NotImplementedException();
        }

        //        DELETE FROM LOC_JobList
        //WHERE(ID = @JoblistID)
        public static void DeleteJobListbyID(this LocalizationContext context, int idJobList)
        {
            var itemToRemove = context.LocJobLists.Find(idJobList);
            context.LocJobLists.Remove(itemToRemove);
            context.SaveChanges();
        }

        //        INSERT INTO[dbo].[LOC_JobList] ([JobName], [UserName], [IDIsoCoding]) VALUES(@JobName, @UserName, @IDIsoCoding);
        //        SELECT ID, JobName, UserName, IDIsoCoding FROM LOC_JobList WHERE(ID = SCOPE_IDENTITY())
        //SELECT @@IDENTITY
        public static int InsertNewJoblist(this LocalizationContext context, string JobName, string UserName, int IDIsoCoding)
        {
            // ANTO check ID = SCOPE_IDENTITY() meaning and query in general
            context.LocJobLists.Add(new LocJobList
            {
                JobName = JobName,
                UserName = UserName,
                IdisoCoding = IDIsoCoding
            });

            // ANTO must return idjoblist
            return context.SaveChanges();
        }
    }
}
