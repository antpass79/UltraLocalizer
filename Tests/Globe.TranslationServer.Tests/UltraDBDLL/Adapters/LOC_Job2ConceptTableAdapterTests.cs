using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(LOC_Job2ConceptTableAdapterTests), "Tested all methods")]
    public class LOC_Job2ConceptTableAdapterTests
    {
        [Fact(DisplayName = nameof(AppendConcept2JobList), Skip = "NOT IMPLEMENTED")]
        public void AppendConcept2JobList()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocJob2Concepts.Count();
            context.AppendConcept2JobList(
                MockConstants.LOC_JOBLIST_ID_299,
                MockConstants.LOC_CONCEPT2CONTEXT_ID_10);

            Assert.Equal(count + 1, context.LocStringsacceptables.Count());
        }

        [Fact(Skip = "UNDER INVESTIGATION")]
        public void DeleteJob2Concept()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocJob2Concepts.Count();
            context.DeleteAcceptable(
                MockConstants.LOC_JOBLIST_ID_299);

            Assert.Equal(count - 1, context.LocStringsacceptables.Count());
        }
    }
}
