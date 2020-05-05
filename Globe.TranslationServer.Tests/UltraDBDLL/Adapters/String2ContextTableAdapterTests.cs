﻿using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(String2ContextTableAdapterTests), "Tested all methods")]
    public class String2ContextTableAdapterTests
    {
        [Fact]
        public void DeletebyIDStringIDConcept2Context()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStrings2Context.Count();
            context.DeletebyIDStringIDConcept2Context(
                MockConstants.LOC_STRING2CONTEXT_IDString_10,
                MockConstants.LOC_STRING2CONTEXT_IDConcept2Context_10);

            Assert.Equal(count - 1, context.LocStrings2Context.Count());
        }

        [Fact]
        public void InsertNewStrings2Context()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStrings2Context.Count();
            context.InsertNewStrings2Context(
                MockConstants.LOC_STRING2CONTEXT_IDString_10,
                MockConstants.LOC_STRING2CONTEXT_IDConcept2Context_10);

            Assert.Equal(count + 1, context.LocStrings2Context.Count());
        }
    }
}