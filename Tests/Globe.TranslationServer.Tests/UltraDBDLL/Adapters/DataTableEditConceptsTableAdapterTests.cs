using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(DataTableEditConceptsTableAdapterTests), "Tested all methods")]
    public class DataTableEditConceptsTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetDataByJob) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetDataByJob()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDataByJob(MockConstants.LOC_JOBLIST_ID_299);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetDatabyComponentInternalJob) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetDatabyComponentInternalJob()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDatabyComponentInternalJob(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.INTERNAL_NAMESPACE_VASCULAR,
                MockConstants.LOC_JOBLIST_ID_299);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetEditDataByComponentInternal) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetEditDataByComponentInternal()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetEditDataByComponentInternal(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.INTERNAL_NAMESPACE_VASCULAR);

            Assert.True(result.Count() > 0);
        }
    }
}
