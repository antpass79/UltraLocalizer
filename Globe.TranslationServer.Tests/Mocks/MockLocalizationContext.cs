﻿using Globe.TranslationServer.Entities;
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

            context.Setup(c => c.Database).Returns(databaseFacade);

            context.Setup(c => c.LocConceptsTable).Returns(mockLocConceptsTable);
            context.Setup(c => c.LocStrings).Returns(mockLocStrings);
            context.Setup(c => c.LocConcept2Context).Returns(mockLocConcept2Contexts);
            context.Setup(c => c.LocContexts).Returns(mockLocContexts);

            return context;
        }

        #endregion
    }
}
