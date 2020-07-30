using System;
using static Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.UltraDBStrings;

namespace Globe.TranslationServer.Extensions
{
    public static class StringExtensions
    {
        public static Languages GetLanguage(this string value)
        {
            Languages language = Languages.en;
            try
            {
                language = (Languages)Enum.Parse(typeof(Languages), value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return language;
        }
    }
}
