using Moq;
using System.Data.Common;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockDbConnection
    {
        public Mock<DbConnection> Mock()
        {
            var dbConnection = new Mock<DbConnection>();
            dbConnection.Setup(c => c.ConnectionString).Returns("Data Source=PC\\SQLEXPRESS;Initial Catalog=Localization;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return dbConnection;
        }
    }
}
