using System;

namespace Globe.TranslationServer.Porting.UltraDBDLL.XmlManager
{
    // ANTO fake
    public class LogManager
    {
        public void Log(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
        public void Log(string text)
        {
            Console.WriteLine(text);
        }
    }
}
