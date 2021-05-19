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

        public UltraDBExtendedStrings()
        {

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
    }
}
