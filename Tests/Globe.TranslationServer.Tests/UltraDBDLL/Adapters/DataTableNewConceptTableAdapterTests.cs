using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.Adapters;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using System.Linq;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(DataTableNewConceptTableAdapterTests), "Tested all methods")]
    public class DataTableNewConceptTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetNewConceptAndContextIDbyComponent) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetNewConceptAndContextIDbyComponent()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetNewConceptAndContextIDbyComponent(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetNewConceptAndContextIDbyComponentInternal) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetNewConceptAndContextIDbyComponentInternal()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetNewConceptAndContextIDbyComponentInternal(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.INTERNAL_NAMESPACE_VASCULAR);

            Assert.True(result.Count() > 0);
        }
    }
}
