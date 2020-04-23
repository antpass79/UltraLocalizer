using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Services;
using Globe.TranslationServer.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Globe.TranslationServer.Tests
{
    public class ExtendedStringAdapterServiceTests
    {
        [Fact]
        async public Task GetAllAsyncWithParameters()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocalizationContext>();
            optionsBuilder.UseSqlServer(MockConstants.CONNECTION_STRING);

            //using var context = new MockLocalizationContext().Mock().Object;
            using var context = new LocalizationContext(optionsBuilder.Options);

            var ultraDBEditConcept = new UltraDBEditConcept(context);
            IAsyncExtendedStringService extendedStringService = new ExtendedStringAdapterService(ultraDBEditConcept);

            var result = await extendedStringService.GetAllAsync("MeasureComponent", "VASCULAR", "en", 674, 1);
            Assert.True(result.Count() > 0);
        }
    }
}
