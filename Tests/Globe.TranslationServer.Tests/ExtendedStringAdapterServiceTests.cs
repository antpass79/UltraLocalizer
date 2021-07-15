using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Porting.UltraDBDLL.UltraDBGlobal;
using MyLabLocalizer.LocalizationService.Services;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyLabLocalizer.LocalizationService.Tests
{
    [Trait(nameof(ExtendedStringAdapterServiceTests), "NOT Tested all methods")]
    public class ExtendedStringAdapterServiceTests
    {
        DbContextOptionsBuilder<LocalizationContext> _optionsBuilder;
        public ExtendedStringAdapterServiceTests()
        {
            _optionsBuilder = new DbContextOptionsBuilder<LocalizationContext>();
            _optionsBuilder.UseSqlServer(MockConstants.CONNECTION_STRING);
        }

        [Fact(Skip = "SQL CONNECTION REQUIRED")]
        async public Task GetAllAsyncWithParameters()
        {
            //using var context = new MockLocalizationContext().Mock().Object;
            using var context = new LocalizationContext(_optionsBuilder.Options);

            var ultraDBEditConcept = new UltraDBEditConcept(context);
            IAsyncExtendedStringService extendedStringService = new ExtendedStringAdapterService(ultraDBEditConcept);

            var result = await extendedStringService.GetAllAsync(
                MockConstants.COMPONENT_NAMESPACE_MEASURECOMPONENT,
                MockConstants.INTERNAL_NAMESPACE_VASCULAR,
                MockConstants.ISO_CODING_EN,
                MockConstants.LOC_JOBLIST_ID_10,
                MockConstants.LOC_CONCEPTSTABLE_ID_10);

            Assert.True(result.Count() > 0);
        }
    }
}
