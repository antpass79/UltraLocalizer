using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System.Collections.Generic;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings
{
    public class UltraDBStrings
    {
        private readonly LocalizationContext context;

        public UltraDBStrings(LocalizationContext context)
        {
            this.context = context;
        }

        public int InsertNewString(int IDLanguage, int IDType, string DataString)
        {
            return context.InsertNewString(IDLanguage, IDType, DataString);
        }

        public List<DBStrings> GetConceptContextEquivalentStrings(int IDString)
        {
            List<DBStrings> lDbStrings = new List<DBStrings>();
            IEnumerable<STRING> dt = context.GetConceptContextEquivalentStrings(IDString);
            foreach (var row in dt)
            {
                DBStrings db = new DBStrings();
                db.DataString = row.String;
                db.IDLanguage = row.IDLanguage;
                db.IDString = row.ID;
                db.IDType = row.IDType;
                db.IDString2Context = row.IDString2Context;
                lDbStrings.Add(db);
            }
            return lDbStrings;
        }

        public List<DBStrings> GetConcept2ContextStrings(int IDConcept2Context)
        {
            List<DBStrings> lDbStrings = new List<DBStrings>();
            IEnumerable<STRING> dt = context.GetDataByConcept2ContextStrings(IDConcept2Context);
            foreach (var row in dt)
            {
                DBStrings db = new DBStrings();
                db.DataString = row.String;
                db.IDLanguage = row.IDLanguage;
                db.IDString = row.ID;
                db.IDType = row.IDType;
                db.IDString2Context = row.IDString2Context;
                lDbStrings.Add(db);
            }
            return lDbStrings;
        }
    }
}
