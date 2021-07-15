using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.Adapters;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using System.Linq;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(InternalConceptsTableTableAdapterTests), "Tested all methods")]
    public class InternalConceptsTableTableAdapterTests
    {
        [Fact]
        public void GetInternalByComponent()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetInternalByComponent(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT);

            Assert.True(result.Count() > 0);
        }
    }
}
