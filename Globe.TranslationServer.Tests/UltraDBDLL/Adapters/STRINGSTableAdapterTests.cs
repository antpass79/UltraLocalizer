using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    public class STRINGSTableAdapterTests
    {
        DbContextOptionsBuilder<LocalizationContext> _optionsBuilder;

        public STRINGSTableAdapterTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<LocalizationContext>();
            _optionsBuilder.UseSqlServer(MockConstants.CONNECTION_STRING);
        }

        [Fact]
        public void GetDataByConcept2ContextStrings()
        {
            using var context = new LocalizationContext(_optionsBuilder.Options);
            var result = context.GetDataByConcept2ContextStrings(10);

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void GetConceptContextEquivalentStrings()
        {
            using var context = new LocalizationContext(_optionsBuilder.Options);
            var result = context.GetConceptContextEquivalentStrings(10);

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void GetStringByID()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetStringByID(10);

            Assert.True(result.Count() == 1);
        }

        [Fact]
        public void UpdatebyID()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetStringByID(10);

            var stringValue = result.ElementAt(0).String;
            context.UpdatebyID("stringValue after update", result.ElementAt(0).ID);

            var updatedResult = context.GetStringByID(10);

            Assert.Equal("stringValue after update", updatedResult.ElementAt(0).String);
        }

        [Fact]
        public void InsertNewString()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStrings.Count();
            context.InsertNewString(7, 1, "Inserted new string");

            Assert.Equal(count + 1, context.LocStrings.Count());
        }
    }
}
