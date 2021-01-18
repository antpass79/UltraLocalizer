using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockLocStrings
    {
        public Mock<DbSet<LocString>> Mock()
        {
            var locStrings = GetLocStrings();
            var queryableLocStrings = locStrings.AsQueryable();
            var dbSet = new Mock<DbSet<LocString>>();

            dbSet.As<IQueryable<LocString>>().Setup(m => m.Provider).Returns(queryableLocStrings.Provider);
            dbSet.As<IQueryable<LocString>>().Setup(m => m.Expression).Returns(queryableLocStrings.Expression);
            dbSet.As<IQueryable<LocString>>().Setup(m => m.ElementType).Returns(queryableLocStrings.ElementType);
            dbSet.As<IQueryable<LocString>>().Setup(m => m.GetEnumerator()).Returns(queryableLocStrings.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocString>())).Callback<LocString>((s) => locStrings.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocString>())).Callback<LocString>((s) => locStrings.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locStrings.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocString> GetLocStrings()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocString) + ".csv");
            var items = CsvParser.Parse<LocStringsForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocString
                {
                    Id = item.ID,
                    Idlanguage = item.IDLanguage,
                    Idtype = item.IDType,
                    String = item.String
                };
            });

            return items.ToList();
        }
    }
}
