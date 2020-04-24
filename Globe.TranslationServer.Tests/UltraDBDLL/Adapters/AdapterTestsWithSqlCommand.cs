using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Mocks;
using Microsoft.EntityFrameworkCore;

namespace Globe.TranslationServer.Tests.UltraDBDLL.Adapters
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
