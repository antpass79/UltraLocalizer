using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.Adapters;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using System.Linq;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(DataTableExtendedStringTableAdapterTests), "Tested all methods")]
    public class DataTableExtendedStringTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetStringByConcept2ContextISO) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
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
