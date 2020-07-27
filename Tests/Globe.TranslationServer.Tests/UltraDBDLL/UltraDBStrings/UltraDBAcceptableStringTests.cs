﻿using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Mocks;
using Globe.TranslationServer.Tests.UltraDBDLL.Adapters;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.UltraDBStrings
{
    [Trait(nameof(UltraDBAcceptableStringTests), "Tested all methods")]
    public class UltraDBAcceptableStringTests : AdapterTestsWithSqlCommand
    {
        [Fact]
        public void InsertNewAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var ultraDBAcceptableString = new Porting.UltraDBDLL.UltraDBStrings.UltraDBAcceptableString(context);

            var beforeDeleteItem = context.LocStringsacceptable.First(item => item.IdString == MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            ultraDBAcceptableString.DeleteAcceptable(MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);
            var afterDeleteItem = context.LocStringsacceptable.First(item => item.IdString == MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            ultraDBAcceptableString.InsertNewAcceptable(MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);
            var afterInsertItem = context.LocStringsacceptable.First(item => item.IdString == MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.NotNull(beforeDeleteItem);
            Assert.Null(afterDeleteItem);
            Assert.NotNull(afterInsertItem);
        }

        [Fact(DisplayName = nameof(isAcceptable) + " - SqlCommand")]
        public void isAcceptable()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var ultraDBAcceptableString = new Porting.UltraDBDLL.UltraDBStrings.UltraDBAcceptableString(context);

            var result = ultraDBAcceptableString.isAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.True(result);
        }

        [Fact]
        public void DeleteAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var ultraDBAcceptableString = new Porting.UltraDBDLL.UltraDBStrings.UltraDBAcceptableString(context);

            var count = context.LocStringsacceptable.Count();
            ultraDBAcceptableString.DeleteAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.Equal(count - 1, context.LocStringsacceptable.Count());
        }
    }
}