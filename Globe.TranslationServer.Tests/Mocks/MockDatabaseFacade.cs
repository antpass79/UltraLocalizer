using Globe.TranslationServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System.Data.Common;

namespace Globe.TranslationServer.Tests.Mocks
{
    public class ExtensionDatabaseFacade : DatabaseFacade
    {
        public ExtensionDatabaseFacade(DbContext context)
            : base(context)
        {
        }

        public ExtensionDatabaseFacade()
            : base(new LocalizationContext())
        {
        }

        virtual public DbConnection GetDbConnection()
        {
            return new MockDbConnection().Mock().Object;
        }
    }

    class MockDatabaseFacade
    {
        public Mock<ExtensionDatabaseFacade> Mock()
        {
            var databaseFacade = new Mock<ExtensionDatabaseFacade>();
            var dbConnection = new MockDbConnection().Mock().Object;
            databaseFacade.Setup(c => c.GetDbConnection()).Returns(dbConnection);

            return databaseFacade;
        }
    }
}
