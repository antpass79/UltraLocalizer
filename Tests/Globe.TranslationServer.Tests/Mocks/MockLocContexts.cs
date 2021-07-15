using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyLabLocalizer.LocalizationService.Tests.Mocks
{
    class MockLocContexts
    {
        public Mock<DbSet<LocContext>> Mock()
        {
            var locContexts = GetLocStrings();
            var queryableLocContexts = locContexts.AsQueryable();
            var dbSet = new Mock<DbSet<LocContext>>();

            dbSet.As<IQueryable<LocContext>>().Setup(m => m.Provider).Returns(queryableLocContexts.Provider);
            dbSet.As<IQueryable<LocContext>>().Setup(m => m.Expression).Returns(queryableLocContexts.Expression);
            dbSet.As<IQueryable<LocContext>>().Setup(m => m.ElementType).Returns(queryableLocContexts.ElementType);
            dbSet.As<IQueryable<LocContext>>().Setup(m => m.GetEnumerator()).Returns(queryableLocContexts.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocContext>())).Callback<LocContext>((s) => locContexts.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocContext>())).Callback<LocContext>((s) => locContexts.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locContexts.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocContext> GetLocStrings()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocContext) + ".csv");
            var items = CsvParser.Parse<LocContextsForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocContext
                {
                    Id = item.ID,
                    ContextName = item.ContextName
                };
            });

            return items.ToList();
        }
    }
}
