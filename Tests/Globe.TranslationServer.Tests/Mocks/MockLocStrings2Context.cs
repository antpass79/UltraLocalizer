using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockLocStrings2Context
    {
        public Mock<DbSet<LocStrings2Context>> Mock()
        {
            var locStrings2Context = GetLocStrings2Context();
            var queryableLocStrings2Context = locStrings2Context.AsQueryable();
            var dbSet = new Mock<DbSet<LocStrings2Context>>();

            dbSet.As<IQueryable<LocStrings2Context>>().Setup(m => m.Provider).Returns(queryableLocStrings2Context.Provider);
            dbSet.As<IQueryable<LocStrings2Context>>().Setup(m => m.Expression).Returns(queryableLocStrings2Context.Expression);
            dbSet.As<IQueryable<LocStrings2Context>>().Setup(m => m.ElementType).Returns(queryableLocStrings2Context.ElementType);
            dbSet.As<IQueryable<LocStrings2Context>>().Setup(m => m.GetEnumerator()).Returns(queryableLocStrings2Context.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocStrings2Context>())).Callback<LocStrings2Context>((s) => locStrings2Context.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocStrings2Context>())).Callback<LocStrings2Context>((s) => locStrings2Context.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locStrings2Context.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocStrings2Context> GetLocStrings2Context()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocStrings2Context) + ".csv");
            var items = CsvParser.Parse<LocStrings2ContextForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocStrings2Context
                {
                    Id = item.ID,
                    Idstring = item.IDString,
                    Idconcept2Context = item.IDConcept2Context
                };
            });

            return items.ToList();
        }
    }
}
