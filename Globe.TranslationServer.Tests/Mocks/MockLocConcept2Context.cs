using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockLocConcept2Context
    {
        public Mock<DbSet<LocConcept2Context>> Mock()
        {
            var locConcept2Contexts = GetLocConcept2Context();
            var queryableLocConcept2Contexts = locConcept2Contexts.AsQueryable();
            var dbSet = new Mock<DbSet<LocConcept2Context>>();

            dbSet.As<IQueryable<LocConcept2Context>>().Setup(m => m.Provider).Returns(queryableLocConcept2Contexts.Provider);
            dbSet.As<IQueryable<LocConcept2Context>>().Setup(m => m.Expression).Returns(queryableLocConcept2Contexts.Expression);
            dbSet.As<IQueryable<LocConcept2Context>>().Setup(m => m.ElementType).Returns(queryableLocConcept2Contexts.ElementType);
            dbSet.As<IQueryable<LocConcept2Context>>().Setup(m => m.GetEnumerator()).Returns(queryableLocConcept2Contexts.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocConcept2Context>())).Callback<LocConcept2Context>((s) => locConcept2Contexts.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocConcept2Context>())).Callback<LocConcept2Context>((s) => locConcept2Contexts.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locConcept2Contexts.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocConcept2Context> GetLocConcept2Context()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocConcept2Context) + ".csv");
            var items = CsvParser.Parse<LocConcept2ContextForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocConcept2Context
                {
                    Id = item.ID,
                    Idconcept = item.IDConcept,
                    Idcontext = item.IDContext,
                };
            });

            return items.ToList();
        }
    }
}
