using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    public class DataTable1TableAdapterTests
    {
        [Fact]
        public void GetConceptAndContextData()
        {
            var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetConceptAndContextData();

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void GetConcept2ContextIDsByConceptTableID()
        {
            var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetConcept2ContextIDsByConceptTableID(MockConstants.LOC_CONCEPTSTABLE_ID_10);

            Assert.True(result.Count() > 0);
        }
    }
}
