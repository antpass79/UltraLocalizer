using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(ComponentConceptsTableTableAdapterTests), "Tested all methods")]
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
