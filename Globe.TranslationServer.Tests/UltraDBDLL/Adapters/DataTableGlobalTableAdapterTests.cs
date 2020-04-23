using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
{
    public class DataTableGlobalTableAdapterTests
    {
        [Fact]
        public void GetEngDatabyComponentInternal()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocalizationContext>();
            optionsBuilder.UseSqlServer(MockConstants.CONNECTION_STRING);

            using var context = new LocalizationContext(optionsBuilder.Options);

            var result = context.GetEngDatabyComponentInternal(674, "MeasureComponent", "VASCULAR");
            Assert.True(result.Count() > 0);
        }
    }
}
