using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.Adapters;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(ComponentConceptsTableTableAdapterTests), "MOCKED")]
    public class ComponentConceptsTableTableAdapterTests
    {
        [Fact]
        public void GetAllComponentName()
        {
            var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetAllComponentName();

            Assert.True(result.Count > 0);
        }
    }
}
