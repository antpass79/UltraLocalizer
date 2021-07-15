using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters;
using System.Linq;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.UltraDBStrings
{
    [Trait(nameof(UltraDBAcceptableStringTests), "Tested all methods")]
    public class UltraDBAcceptableStringTests : AdapterTestsWithSqlCommand
    {
        [Fact(Skip = "UNDER INVESTIGATION")]
        public void InsertNewAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var ultraDBAcceptableString = new Porting.UltraDBDLL.UltraDBStrings.UltraDBAcceptableString(context);

            var beforeDeleteItem = context.LocStringsacceptables.First(item => item.IdString == MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            ultraDBAcceptableString.DeleteAcceptable(MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);
            var afterDeleteItem = context.LocStringsacceptables.First(item => item.IdString == MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            ultraDBAcceptableString.InsertNewAcceptable(MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);
            var afterInsertItem = context.LocStringsacceptables.First(item => item.IdString == MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.NotNull(beforeDeleteItem);
            Assert.Null(afterDeleteItem);
            Assert.NotNull(afterInsertItem);
        }

        [Fact(DisplayName = nameof(isAcceptable) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void isAcceptable()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var ultraDBAcceptableString = new Porting.UltraDBDLL.UltraDBStrings.UltraDBAcceptableString(context);

            var result = ultraDBAcceptableString.isAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.True(result);
        }

        [Fact(Skip = "UNDER INVESTIGATION")]
        public void DeleteAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var ultraDBAcceptableString = new Porting.UltraDBDLL.UltraDBStrings.UltraDBAcceptableString(context);

            var count = context.LocStringsacceptables.Count();
            ultraDBAcceptableString.DeleteAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.Equal(count - 1, context.LocStringsacceptables.Count());
        }
    }
}
