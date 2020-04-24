using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(DataTableExtendedStringTableAdapterTests), "Tested all methods")]
    public class DataTableExtendedStringTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetStringByConcept2ContextISO) + " - SqlCommand")]
        public void GetStringByConcept2ContextISO()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetStringByConcept2ContextISO(
                MockConstants.LOC_CONCEPT2CONTEXT_ID_10,
                MockConstants.ISO_CODING_EN);

            Assert.True(result.Count() > 0);
        }
    }
}
