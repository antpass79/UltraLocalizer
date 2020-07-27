using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept
{
    public class UltraDBConcept2Context
    {
        private readonly LocalizationContext context;

        public UltraDBConcept2Context(LocalizationContext context)
        {
            this.context = context;
        }

        public int InsertNewConcept2Context(int IDConcept, int IDContext)
        {
            return context.InsertNewConcept2Context(IDConcept, IDContext);
        }
    }
}
