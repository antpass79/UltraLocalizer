using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.Adapters;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using System.Linq;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(LOC_STRINGSAcceptableTableAdapterTests), "Tested all methods")]
    public class LOC_STRINGSAcceptableTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(Skip = "UNDER INVESTIGATION")]
        public void DeleteAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStringsacceptables.Count();
            context.DeleteAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.Equal(count - 1, context.LocStringsacceptables.Count());
        }

        [Fact]
        public void InsertNewAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStringsacceptables.Count();
            context.InsertNewAcceptable(
                MockConstants.LOC_STRING2CONTEXT_IDString_10);

            Assert.Equal(count + 1, context.LocStringsacceptables.Count());
        }

        [Fact(DisplayName = nameof(isAcceptable) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void isAcceptable()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);

            var result = context.isAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.True(result);
        }        
    }
}
