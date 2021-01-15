using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal
{
    public class UltraDBJobList
    {
        private readonly LocalizationContext context;
        private readonly IConfiguration configuration;

        public UltraDBJobList(LocalizationContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public List<JobList> GetAllJobListByUserNameIso(string UserName, string isocoding, bool isMaster)
        {
            //bool l_isMaster = (Roles.IsUserInRole(UserName, "MasterTranslator") ||
            //                  Roles.IsUserInRole(UserName, "Admin"));

            //var user = await userManager.FindByNameAsync(UserName);
            //bool l_isMaster = await userManager.IsInRoleAsync(user, "MasterTranslator") || await userManager.IsInRoleAsync(user, "Admin");

            // ANTO check roles before
            if (!isMaster)
            {
                int idIso = (int)UltraDBStrings.UltraDBStrings.ParseFromString(isocoding);
                var dt = context.GetDataByUserNameIDISO(UserName, idIso);
                List<JobList> retList = (from p in dt
                                         select new JobList { IDJob = p.ID, JobName = p.JobName, IDIso = idIso }).Distinct().ToList();
                return retList;
            }
            else
            {
                int idIso = (int)UltraDBStrings.UltraDBStrings.ParseFromString(isocoding);
                var dt = context.GetDataByIDIso(idIso);
                List<JobList> retList = (from p in dt
                                         where p.IDIsoCoding == idIso
                                         select new JobList { IDJob = p.ID, JobName = p.UserName + "-" + p.JobName, IDIso = idIso }).Distinct().ToList();
                return retList;
            }
        }

        public List<JobList> GetAllJobListByUserNameIso_OLD(string UserName, string isocoding)
        {
            //bool l_isMaster = (Roles.IsUserInRole(UserName, "MasterTranslator") ||
            //                  Roles.IsUserInRole(UserName, "Admin"));

            //var user = await userManager.FindByNameAsync(UserName);
            //bool l_isMaster = await userManager.IsInRoleAsync(user, "MasterTranslator") || await userManager.IsInRoleAsync(user, "Admin");

            // ANTO check roles before
            bool l_isMaster = true;
            if (!l_isMaster)
            {
                int idIso = (int)UltraDBStrings.UltraDBStrings.ParseFromString(isocoding);
                var dt = context.GetDataByUserNameIDISO(UserName, idIso);
                List<JobList> retList = (from p in dt
                                         select new JobList { IDJob = p.ID, JobName = p.JobName, IDIso = idIso }).Distinct().ToList();
                return retList;
            }
            else
            {
                int idIso = (int)UltraDBStrings.UltraDBStrings.ParseFromString(isocoding);
                var dt = context.GetDataByIDIso(idIso);
                List<JobList> retList = (from p in dt
                                         where p.IDIsoCoding == idIso
                                         select new JobList { IDJob = p.ID, JobName = p.UserName + "-" + p.JobName, IDIso = idIso }).Distinct().ToList();
                return retList;
            }
        }

        public void CreateJobList(string User, string isocoding)
        {
            // ANTO must return idjoblist
            int IDJobList = context.InsertNewJoblist("default", User, (int)UltraDBStrings.UltraDBStrings.ParseFromString(isocoding));
            UltraDBJob2Concept jb2c = new UltraDBJob2Concept(context);
            if (isocoding == "en")
            {
                UltraDBNewConcept c = new UltraDBNewConcept(context);
                List<GroupedStringEntity> ge = c.GetGroupledNewDataBy("all", "");
                foreach (GroupedStringEntity item in ge)
                {
                    foreach (StringEntity ent in item.Group)
                    {
                        jb2c.AppendConcept2JobList(IDJobList, ent.IDConcept2Context);
                    }
                }
            }
            else
            {
                UltraDBGlobal c = new UltraDBGlobal(context);
                List<GroupedStringEntity> ge = c.GetGroupledMissingDataBy("all", "", isocoding);
                foreach (GroupedStringEntity item in ge)
                {
                    foreach (StringEntity ent in item.Group)
                    {
                        jb2c.AppendConcept2JobList(IDJobList, ent.IDConcept2Context);
                    }
                }
            }
        }

        public void EraseJobList(string User, string Isocoding)
        {
            var dt = context.GetDataByUserISO(User, (int)UltraDBStrings.UltraDBStrings.ParseFromString(Isocoding));
            if (dt != null && dt.Count() > 0)
            {
                UltraDBJob2Concept j2c = new UltraDBJob2Concept(context);
                j2c.DeleteJob2Concept(dt.ElementAt(0).ID);
                context.Delete(dt.ElementAt(0).ID, dt.ElementAt(0).JobName, dt.ElementAt(0).UserName, dt.ElementAt(0).IDIsoCoding);
            }
        }

        public void EraseJobListbyID(int idJobList)
        {
            UltraDBJob2Concept j2c = new UltraDBJob2Concept(context);
            j2c.DeleteJob2Concept(idJobList);
            context.DeleteJobListbyID(idJobList);
        }
    }
}
