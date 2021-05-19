using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.DataTables;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings
{
    public class UltraDBExtendedStrings
    {
        public enum Languages { en = 1, fr = 2, it = 3, de = 4, es = 5, zh = 6, ru = 7, pt = 8 };
        public enum StringTypes { String = 1, Abbreviation = 2, Label = 3 };

        private readonly LocalizationContext context;

        public UltraDBExtendedStrings(LocalizationContext context)
        {
            this.context = context;
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

        //public DBExtendedStrings GetStringByConcept2ContextISO(int Concept2Context, string ISO)
        //{
        //    IEnumerable<DataTableExtendedStrings> dt = context.GetStringByConcept2ContextISO(Concept2Context, ISO);
        //    if (dt.Count() == 0)
        //        return null;
        //    DBExtendedStrings ex = new DBExtendedStrings();
        //    ex.IDString = dt.ElementAt(0).ID;
        //    ex.ContextName = dt.ElementAt(0).ContextName;
        //    ex.DataString = dt.ElementAt(0).String;
        //    ex.IDConcept2Context = dt.ElementAt(0).IDConcept2Context;
        //    ex.IDLanguage = dt.ElementAt(0).IDLanguage;
        //    ex.IDTYpe = dt.ElementAt(0).IDType;
        //    ex.Is2Translate = dt.ElementAt(0).Is2Translate;
        //    ex.IsLocked = dt.ElementAt(0).IsLocked;
        //    ex.ISOCoding = dt.ElementAt(0).ISOCoding;
        //    ex.StringType = dt.ElementAt(0).Type;
        //    ex.IDContext2String = dt.ElementAt(0).IDString2Context;
        //    return ex;
        //}
    }
}
