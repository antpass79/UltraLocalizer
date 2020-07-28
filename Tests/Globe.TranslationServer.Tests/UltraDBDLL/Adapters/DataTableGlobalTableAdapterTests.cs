using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(DataTableGlobalTableAdapterTests), "Tested all methods")]
    public class DataTableGlobalTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetEngDatabyComponentInternal) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetEngDatabyComponentInternal()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetEngDatabyComponentInternal(
                MockConstants.LOC_JOBLIST_ID_10,
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.INTERNAL_NAMESPACE_VASCULAR);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetMissingDataByComponentISO) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetMissingDataByComponentISO()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetMissingDataByComponentISO(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.ISO_CODING_EN);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetMissingDataByComponentISOInternal) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetMissingDataByComponentISOInternal()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetMissingDataByComponentISOInternal(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.INTERNAL_NAMESPACE_VASCULAR,
                MockConstants.ISO_CODING_EN);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetMissingDataByConceptID) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetMissingDataByConceptID()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetMissingDataByConceptID(
                MockConstants.LOC_CONCEPTSTABLE_ID_10,
                MockConstants.ISO_CODING_EN);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetDatabyComponentISO) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetDatabyComponentISO()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDatabyComponentISO(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.ISO_CODING_EN);

            Assert.True(result.Count() > 0);
        }
    }
}
