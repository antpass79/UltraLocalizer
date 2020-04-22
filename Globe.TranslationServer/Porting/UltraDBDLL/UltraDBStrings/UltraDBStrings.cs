using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings
{
    public class UltraDBStrings
    {
        public enum StringTypes { String = 1, Abbreviation = 2, Label = 3 };
        public enum Languages { en = 1, fr = 2, it = 3, de = 4, es = 5, zh = 6, ru = 7, pt = 8 };

        private readonly LocalizationContext context;

        public UltraDBStrings(LocalizationContext context)
        {
            this.context = context;
        }

        public int InsertNewString(int IDLanguage, int IDType, string DataString)
        {
            return context.InsertNewString(IDLanguage, IDType, DataString);
        }

        public List<DBLanguage> Getlanguage()
        {
            List<DBLanguage> db = new List<DBLanguage>();
            foreach (int value in System.Enum.GetValues(typeof(UltraDBStrings.Languages)))
            {
                DBLanguage d = new DBLanguage();
                d.IDLanguage = value;
                d.DataString = ((Languages)value).ToString();
                db.Add(d);
            }
            return db;
        }

        static public Languages ParseFromString(string value)
        {
            Languages pet = Languages.en;
            try
            {
                pet = (Languages)Enum.Parse(typeof(Languages), value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return pet;
        }

        public void UpdateString(int ID, string DataString)
        {
            context.UpdatebyID(DataString, ID);
        }


        public DBStrings GetStringbyID(int ID)
        {
            DBStrings db = new DBStrings();
            IEnumerable<STRING> dt = context.GetStringByID(ID);
            if (dt != null && dt.Count() > 0)
            {
                db.DataString = dt.ElementAt(0).String;
                db.IDLanguage = dt.ElementAt(0).IDLanguage;
                db.IDString = dt.ElementAt(0).ID;
                db.IDType = dt.ElementAt(0).IDType;
            }
            else
            {
                db.DataString = string.Empty;
                db.IDLanguage = 0;
                db.IDString = 0;
                db.IDType = 0;
            }
            return db;
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
