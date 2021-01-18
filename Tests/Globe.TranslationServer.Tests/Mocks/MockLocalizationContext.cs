using Globe.TranslationServer.Entities;
using Moq;

namespace Globe.TranslationServer.Tests.Mocks
{
    public class MockLocalizationContext
    {
        #region Public Functions

        public Mock<LocalizationContext> Mock()
        {
            var context = new Mock<LocalizationContext>();

            var databaseFacade = new MockDatabaseFacade().Mock().Object;

            var mockLocConceptsTable = new MockLocConceptsTable().Mock().Object;
            var mockLocStrings = new MockLocStrings().Mock().Object;
            var mockLocConcept2Contexts = new MockLocConcept2Context().Mock().Object;
            var mockLocContexts = new MockLocContexts().Mock().Object;
            var mockLocJobList = new MockLocJobList().Mock().Object;
            var mockLocStringsacceptable = new MockLocStringsacceptable().Mock().Object;
            var mockLocStrings2Context = new MockLocStrings2Context().Mock().Object;

            context.Setup(c => c.Database).Returns(databaseFacade);

            context.Setup(c => c.LocConceptsTables).Returns(mockLocConceptsTable);
            context.Setup(c => c.LocStrings).Returns(mockLocStrings);
            context.Setup(c => c.LocConcept2Contexts).Returns(mockLocConcept2Contexts);
            context.Setup(c => c.LocContexts).Returns(mockLocContexts);
            context.Setup(c => c.LocJobLists).Returns(mockLocJobList);
            context.Setup(c => c.LocStringsacceptables).Returns(mockLocStringsacceptable);
            context.Setup(c => c.LocStrings2Contexts).Returns(mockLocStrings2Context);

            return context;
        }

        #endregion
    }
}
