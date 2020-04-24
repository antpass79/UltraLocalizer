using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(LOC_JobListTableAdapterTests), "Tested all methods")]
    public class LOC_JobListTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetDataByIDIso) + " - SqlCommand")]
        public void GetDataByIDIso()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDataByIDIso(
                MockConstants.LOC_LANGUAGES_ID_1_EN);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetDataByUserISO) + " - SqlCommand")]
        public void GetDataByUserISO()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDataByUserISO(
                MockConstants.USERNAME_MARCODELPIANO,
                MockConstants.LOC_LANGUAGES_ID_1_EN);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetDataByUserNameIDISO) + " - SqlCommand")]
        public void GetDataByUserNameIDISO()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDataByUserNameIDISO(
                MockConstants.USERNAME_MARCODELPIANO,
                MockConstants.LOC_LANGUAGES_ID_1_EN);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(Delete) + " - Not found the original code")]
        public void Delete()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var result = context.Delete(
                -1,
                string.Empty,
                string.Empty,
                -1);

            Assert.True(false);
        }

        [Fact]
        public void DeleteJobListbyID()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocJobList.Count();
            context.DeleteJobListbyID(
                MockConstants.LOC_JOBLIST_ID_299);

            Assert.Equal(count - 1, context.LocJobList.Count());
        }

        [Fact(DisplayName = nameof(InsertNewJoblist) + " - Must return id job list")]
        public void InsertNewJoblist()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocJobList.Count();
            context.InsertNewJoblist(
                MockConstants.JOB_NAME_FAKE,
                MockConstants.USERNAME_MARCODELPIANO,
                MockConstants.LOC_LANGUAGES_ID_1_EN);

            Assert.Equal(count + 1, context.LocJobList.Count());
        }        
    }
}
