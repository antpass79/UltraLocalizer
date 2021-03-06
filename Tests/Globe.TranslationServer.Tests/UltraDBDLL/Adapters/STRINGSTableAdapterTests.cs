﻿using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.Adapters;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    [Trait(nameof(STRINGSTableAdapterTests), "Tested all methods")]
    public class STRINGSTableAdapterTests : AdapterTestsWithSqlCommand
    {
        [Fact(DisplayName = nameof(GetDataByConcept2ContextStrings) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetDataByConcept2ContextStrings()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetDataByConcept2ContextStrings(
                MockConstants.LOC_STRING2CONTEXT_IDConcept2Context_10);

            Assert.True(result.Count() > 0);
        }

        [Fact(DisplayName = nameof(GetConceptContextEquivalentStrings) + " - SqlCommand", Skip = "SQL CONNECTION REQUIRED")]
        public void GetConceptContextEquivalentStrings()
        {
            using var context = new LocalizationContext(OptionsBuilder.Options);
            var result = context.GetConceptContextEquivalentStrings(
                MockConstants.LOC_STRING2CONTEXT_IDString_10);

            Assert.True(result.Count() > 0);
        }

        [Fact(Skip = "UNDER INVESTIGATION")]
        public void GetStringByID()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetStringByID(
                MockConstants.LOC_STRINGS_ID_10);

            Assert.True(result.Count() == 1);
            Assert.False(true, "GetStringByID should return a single item because ID is Primary Key");
        }

        [Fact]
        public void UpdatebyID()
        {
            using var context = new MockLocalizationContext().Mock().Object;
            var result = context.GetStringByID(
                MockConstants.LOC_STRINGS_ID_10);

            var stringValue = result.ElementAt(0).String;
            context.UpdatebyID("stringValue after update", result.ElementAt(0).ID);

            var updatedResult = context.GetStringByID(
                MockConstants.LOC_STRINGS_ID_10);

            Assert.Equal("stringValue after update", updatedResult.ElementAt(0).String);
        }

        [Fact(Skip = "UNDER INVESTIGATION")]
        public void InsertNewString()
        {
            using var context = new MockLocalizationContext().Mock().Object;

            var count = context.LocStrings.Count();
            context.InsertNewString(7, 1, "Inserted new string");

            Assert.Equal(count + 1, context.LocStrings.Count());
        }
    }
}
