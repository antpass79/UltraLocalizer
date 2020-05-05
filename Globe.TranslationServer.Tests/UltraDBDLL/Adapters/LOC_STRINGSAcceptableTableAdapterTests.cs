﻿using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(LOC_STRINGSAcceptableTableAdapterTests), "Tested all methods")]
    public class LOC_STRINGSAcceptableTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact]
        public void DeleteAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStringsacceptable.Count();
            context.DeleteAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.Equal(count - 1, context.LocStringsacceptable.Count());
        }

        [Fact]
        public void InsertNewAcceptable()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStringsacceptable.Count();
            context.InsertNewAcceptable(
                MockConstants.LOC_STRING2CONTEXT_IDString_10);

            Assert.Equal(count + 1, context.LocStringsacceptable.Count());
        }

        [Fact(DisplayName = nameof(isAcceptable) + " - SqlCommand")]
        public void isAcceptable()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);

            var result = context.isAcceptable(
                MockConstants.LOC_STRINGSACCEPTABLE_IDString_39955);

            Assert.True(result);
        }        
    }
}