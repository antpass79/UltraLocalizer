using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Mocks;
using Globe.TranslationServer.Tests.UltraDBDLL.Adapters;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.UltraDBStrings
{
    [Trait(nameof(UltraDBStringsTests), "NOT Tested all methods")]
    public class UltraDBStringsTests : AdapterTestsWithSqlCommand
    {
        public UltraDBStringsTests()
        {
        }

        [Fact]
        public void InsertNewString()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var ultraDBStrings = new Porting.UltraDBDLL.UltraDBStrings.UltraDBStrings(context);

            var count = context.LocStrings.Count();
            ultraDBStrings.InsertNewString(7, 1, "Inserted new string");

            Assert.Equal(count + 1, context.LocStrings.Count());
        }

        [Fact]
        public void Getlanguage()
        {
            Assert.True(false);
        }

        [Fact]
        public void ParseFromString()
        {
            Assert.True(false);
        }

        [Fact]
        public void UpdateString()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var ultraDBStrings = new Porting.UltraDBDLL.UltraDBStrings.UltraDBStrings(context);

            var result = ultraDBStrings.GetStringbyID(
                MockConstants.LOC_STRINGS_ID_10);

            var stringValue = result.DataString;
            ultraDBStrings.UpdateString(result.IDString, "stringValue after update");

            var updatedResult = ultraDBStrings.GetStringbyID(
                MockConstants.LOC_STRINGS_ID_10);

            Assert.Equal("stringValue after update", updatedResult.DataString);
        }

        [Fact]
        public void GetStringbyID()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var ultraDBStrings = new Porting.UltraDBDLL.UltraDBStrings.UltraDBStrings(context);

            var result = ultraDBStrings.GetStringbyID(
                MockConstants.LOC_STRINGS_ID_10);

            Assert.True(result != null);
        }

        [Fact]
        public void GetConceptContextEquivalentStrings()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var ultraDBStrings = new Porting.UltraDBDLL.UltraDBStrings.UltraDBStrings(context);

            var result = ultraDBStrings.GetConceptContextEquivalentStrings(
                MockConstants.LOC_STRING2CONTEXT_IDString_10);

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void GetConcept2ContextStrings()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var ultraDBStrings = new Porting.UltraDBDLL.UltraDBStrings.UltraDBStrings(context);

            var result = ultraDBStrings.GetConcept2ContextStrings(
                MockConstants.LOC_STRING2CONTEXT_IDConcept2Context_10);

            Assert.True(result.Count() > 0);
        }
    }
}
