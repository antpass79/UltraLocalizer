using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.Adapters;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using System.Linq;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(DataTable1TableAdapterTests), "Tested all methods")]
    public class DataTable1TableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact]
        public void GetConceptAndContextData()
        {
            var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetConceptAndContextData();

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void GetConcept2ContextIDsByConceptTableID()
        {
            var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetConcept2ContextIDsByConceptTableID(MockConstants.LOC_CONCEPTSTABLE_ID_10);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetDataByConcept2Context) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetDataByConcept2Context()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDataByConcept2Context(
                MockConstants.LOC_CONCEPT2CONTEXT_ID_10);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetComplimentaryDataByComponentInternalISOjob) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetComplimentaryDataByComponentInternalISOjob()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetComplimentaryDataByComponentInternalISOjob(
                MockConstants.LOC_JOBLIST_ID_10,
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.INTERNAL_NAMESPACE_VASCULAR,
                MockConstants.LOC_LANGUAGES_ID_1_EN);

            Assert.True(result.Count() > 0);
        }
    }
}
