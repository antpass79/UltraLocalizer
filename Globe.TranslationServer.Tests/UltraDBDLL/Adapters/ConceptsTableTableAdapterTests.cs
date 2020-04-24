using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(ConceptsTableTableAdapterTests), "Tested all methods")]
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
            context.InsertNewConcept(MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT, MockConstants.INTERNAL_NAMESPACE_VASCULAR, MockConstants.LOCALIZATION_ID_FAKE, MockConstants.IGNORED_TRUE, MockConstants.COMMENT_FAKE);

            Assert.Equal(count + 1, context.LocConceptsTable.Count());
        }

        [Fact]
        public void UpdateConcept()
        {
            var context = new MockLocalizationContext().Mock().Object;

            var item = context.LocConceptsTable.Find(MockConstants.LOC_CONCEPTSTABLE_ID_10);
            context.UpdateConcept(MockConstants.LOC_CONCEPTSTABLE_ID_10, MockConstants.IGNORED_TRUE, MockConstants.COMMENT_FAKE);
            var updatedItem = context.LocConceptsTable.Find(MockConstants.LOC_CONCEPTSTABLE_ID_10);

            Assert.Equal(MockConstants.IGNORED_TRUE, item.Ignore);
            Assert.Equal(MockConstants.IGNORED_TRUE, updatedItem.Ignore);
            Assert.Equal(MockConstants.COMMENT_FAKE, item.Comment);
            Assert.Equal(MockConstants.COMMENT_FAKE, updatedItem.Comment);
        }

        [Fact]
        public void CleanOrphanedConcepts()
        {
            var context = new MockLocalizationContext().Mock().Object;
            context.CleanOrphanedConcepts();

            Assert.True(false);
        }

        [Fact]
        public void GetDataByID()
        {
            var context = new MockLocalizationContext().Mock().Object;

            var result = context.GetDataByID(
                MockConstants.LOC_CONCEPTSTABLE_ID_10);

            Assert.True(result != null);
        }
    }
}
