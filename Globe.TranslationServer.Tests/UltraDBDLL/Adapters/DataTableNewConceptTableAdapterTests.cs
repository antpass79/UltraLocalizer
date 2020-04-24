using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(DataTableNewConceptTableAdapterTests), "Tested all methods")]
    public class DataTableNewConceptTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetNewConceptAndContextIDbyComponent) + " - SqlCommand")]
        public void GetNewConceptAndContextIDbyComponent()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetNewConceptAndContextIDbyComponent(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetNewConceptAndContextIDbyComponentInternal) + " - SqlCommand")]
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
