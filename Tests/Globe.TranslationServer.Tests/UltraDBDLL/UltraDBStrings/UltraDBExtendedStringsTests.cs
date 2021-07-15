using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.UltraDBStrings
{
    [Trait(nameof(UltraDBExtendedStringsTests), "NOT Tested all methods")]
    public class UltraDBExtendedStringsTests : AdapterTestsWithSqlCommand
    {
        [Fact(Skip = "UNDER INVESTIGATION")]
        public void ParseFromString()
        {
            Assert.True(false);
        }

        [Fact(DisplayName = nameof(GetStringByConcept2ContextISO) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
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
