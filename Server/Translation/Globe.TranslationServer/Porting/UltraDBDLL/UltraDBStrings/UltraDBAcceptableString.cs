using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings
{
    public class UltraDBAcceptableString
    {
        private readonly LocalizationContext context;

        public UltraDBAcceptableString(LocalizationContext context)
        {
            this.context = context;
        }

        public void InsertNewAcceptable(int IDString)
        {
            context.DeleteAcceptable(IDString);
            context.InsertNewAcceptable(IDString);
        }

        public bool isAcceptable(int IDString)
        {
            return context.isAcceptable(IDString);
        }

        public void DeleteAcceptable(int IDString)
        {
            context.DeleteAcceptable(IDString);
        }
    }
}
