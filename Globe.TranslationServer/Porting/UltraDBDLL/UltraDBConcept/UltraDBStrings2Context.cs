using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept
{
    public class UltraDBStrings2Context
    {
        private readonly LocalizationContext context;

        public UltraDBStrings2Context(LocalizationContext context)
        {
            this.context = context;
        }

        public void DeletebyIDStringIDConcept2Context(int idString, int idConcept2Context)
        {
            context.DeletebyIDStringIDConcept2Context(idString, idConcept2Context);
        }

        public int InsertNewStrings2Context(int IDString, int IDConcept2Context)
        {
            return context.InsertNewStrings2Context(IDString, IDConcept2Context);
        }
    }
}
