using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Tests.Mocks;
using Microsoft.EntityFrameworkCore;

namespace MyLabLocalizer.LocalizationService.Tests.UltraDBDLL.Adapters
{
    public abstract class AdapterTestsWithSqlCommand
    {
        protected DbContextOptionsBuilder<LocalizationContext> OptionsBuilder { get; }

        public AdapterTestsWithSqlCommand()
        {
            OptionsBuilder = new DbContextOptionsBuilder<LocalizationContext>();
            OptionsBuilder.UseSqlServer(MockConstants.CONNECTION_STRING);
        }
    }
}
