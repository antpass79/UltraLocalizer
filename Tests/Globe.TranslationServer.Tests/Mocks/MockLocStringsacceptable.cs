using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockLocStringsacceptable
    {
        public Mock<DbSet<LocStringsacceptable>> Mock()
        {
            var locStringsacceptable = GetLocStringsacceptable();
            var queryableLocStringsacceptable = locStringsacceptable.AsQueryable();
            var dbSet = new Mock<DbSet<LocStringsacceptable>>();

            dbSet.As<IQueryable<LocStringsacceptable>>().Setup(m => m.Provider).Returns(queryableLocStringsacceptable.Provider);
            dbSet.As<IQueryable<LocStringsacceptable>>().Setup(m => m.Expression).Returns(queryableLocStringsacceptable.Expression);
            dbSet.As<IQueryable<LocStringsacceptable>>().Setup(m => m.ElementType).Returns(queryableLocStringsacceptable.ElementType);
            dbSet.As<IQueryable<LocStringsacceptable>>().Setup(m => m.GetEnumerator()).Returns(queryableLocStringsacceptable.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocStringsacceptable>())).Callback<LocStringsacceptable>((s) => locStringsacceptable.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocStringsacceptable>())).Callback<LocStringsacceptable>((s) => locStringsacceptable.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locStringsacceptable.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocStringsacceptable> GetLocStringsacceptable()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocStringsacceptable) + ".csv");
            var items = CsvParser.Parse<LocStringsacceptableForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocStringsacceptable
                {
                    Id = item.ID,
                    IdString = item.ID_String,
                };
            });

            return items.ToList();
        }
    }
}
