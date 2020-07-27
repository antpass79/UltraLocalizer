using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockLocContexts
    {
        public Mock<DbSet<LocContexts>> Mock()
        {
            var locContexts = GetLocStrings();
            var queryableLocContexts = locContexts.AsQueryable();
            var dbSet = new Mock<DbSet<LocContexts>>();

            dbSet.As<IQueryable<LocContexts>>().Setup(m => m.Provider).Returns(queryableLocContexts.Provider);
            dbSet.As<IQueryable<LocContexts>>().Setup(m => m.Expression).Returns(queryableLocContexts.Expression);
            dbSet.As<IQueryable<LocContexts>>().Setup(m => m.ElementType).Returns(queryableLocContexts.ElementType);
            dbSet.As<IQueryable<LocContexts>>().Setup(m => m.GetEnumerator()).Returns(queryableLocContexts.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocContexts>())).Callback<LocContexts>((s) => locContexts.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocContexts>())).Callback<LocContexts>((s) => locContexts.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locContexts.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocContexts> GetLocStrings()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocContexts) + ".csv");
            var items = CsvParser.Parse<LocContextsForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocContexts
                {
                    Id = item.ID,
                    ContextName = item.ContextName
                };
            });

            return items.ToList();
        }
    }
}
