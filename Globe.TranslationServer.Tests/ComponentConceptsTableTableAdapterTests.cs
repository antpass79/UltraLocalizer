using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using Xunit;

namespace Globe.TranslationServer.Tests
{
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
