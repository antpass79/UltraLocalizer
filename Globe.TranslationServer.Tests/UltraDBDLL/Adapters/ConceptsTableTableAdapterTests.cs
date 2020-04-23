using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    public class ConceptsTableTableAdapterTests
    {
        [Fact]
        public void GetData()
        {
            var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetData();

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void InsertNewConcept()
        {
            var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocConceptsTable.Count();
            context.InsertNewConcept(MockConstants.FAKE_COMPONENT_NAMESPACE, MockConstants.FAKE_INTERNAL_NAMESPACE, MockConstants.FAKE_LOCALIZATION_ID, MockConstants.IGNORED_TRUE, MockConstants.FAKE_COMMENT);

            Assert.Equal(count + 1, context.LocConceptsTable.Count());
        }

        [Fact]
        public void UpdateConcept()
        {
            var context = new MockLocalizationContext().Mock().Object;

            var item = context.LocConceptsTable.Find(MockConstants.LOC_CONCEPTSTABLE_ID_10);
            context.UpdateConcept(MockConstants.LOC_CONCEPTSTABLE_ID_10, MockConstants.IGNORED_TRUE, MockConstants.FAKE_COMMENT);
            var updatedItem = context.LocConceptsTable.Find(MockConstants.LOC_CONCEPTSTABLE_ID_10);

            Assert.Equal(MockConstants.IGNORED_TRUE, item.Ignore);
            Assert.Equal(MockConstants.IGNORED_TRUE, updatedItem.Ignore);
            Assert.Equal(MockConstants.FAKE_COMMENT, item.Comment);
            Assert.Equal(MockConstants.FAKE_COMMENT, updatedItem.Comment);
        }
    }
}
