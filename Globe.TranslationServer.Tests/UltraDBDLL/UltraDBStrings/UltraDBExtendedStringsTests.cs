using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Mocks;
using Globe.TranslationServer.Tests.UltraDBDLL.Adapters;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.UltraDBStrings
{
    [Trait(nameof(UltraDBExtendedStringsTests), "NOT Tested all methods")]
    public class UltraDBExtendedStringsTests : AdapterTestsWithSqlCommand
    {
        [Fact]
        public void ParseFromString()
        {
            Assert.True(false);
        }

        [Fact(DisplayName = nameof(GetStringByConcept2ContextISO) + " - SqlCommand")]
        public void GetStringByConcept2ContextISO()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var ultraDBAcceptableString = new Porting.UltraDBDLL.UltraDBStrings.UltraDBAcceptableString(context);

            var result = ultraDBAcceptableString.isAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.True(result);
        }
    }
}
