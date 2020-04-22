using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;

namespace Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal
{
    public class UltraDBJob2Concept
    {
        private readonly LocalizationContext context;

        public UltraDBJob2Concept(LocalizationContext context)
        {
            this.context = context;
        }

        public void AppendConcept2JobList(int idJobList, int idConcept2Context)
        {
            context.AppendConcept2JobList(idJobList, idConcept2Context);
        }

        public void DeleteJob2Concept(int idJobList)
        {
            context.DeleteJob2Concept(idJobList);
        }
    }
}
