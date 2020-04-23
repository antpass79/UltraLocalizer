using Globe.TranslationServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Globe.TranslationServer.Tests.Mocks
{
    public class MockLocalizationContext
    {
        #region Public Functions

        public Mock<LocalizationContext> Mock()
        {
            var context = new Mock<LocalizationContext>(new object[] {  });

            var mockLocConceptsTable = new MockLocConceptsTable().Mock().Object;
            var databaseFacade = new MockDatabaseFacade().Mock().Object;

            context.Setup(c => c.Database).Returns(databaseFacade);
            context.Setup(c => c.LocConceptsTable).Returns(mockLocConceptsTable);

            return context;
        }

        #endregion
    }
}
