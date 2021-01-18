using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(Concept2ContextTableAdapterTests), "MOCKED")]
    public class Concept2ContextTableAdapterTests
    {
        [Fact]
        public void InsertNewConcept2Context()
        {
            var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocConcept2Contexts.Count();
            context.InsertNewConcept2Context(50, 50);

            Assert.Equal(count + 1, context.LocConcept2Contexts.Count());
        }
    }
}
