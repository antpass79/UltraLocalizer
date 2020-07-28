using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(LOC_Concept2ContextTableAdapterTests), "Tested all methods")]
    public class LOC_Concept2ContextTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact]
        public void GetAllC2CData()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetAllC2CData();

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetSiblingsByIDStringISO) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetSiblingsByIDStringISO()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);

            var result = context.GetSiblingsByIDStringISO(
                MockConstants.LOC_STRING2CONTEXT_IDString_10,
                MockConstants.LOC_LANGUAGES_ID_1_EN);

            Assert.True(result.Count() > 0);
        }
    }
}
