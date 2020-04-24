using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockLocJobList
    {
        public Mock<DbSet<LocJobList>> Mock()
        {
            var locJobList = GetLocJobList();
            var queryableLocJobList = locJobList.AsQueryable();
            var dbSet = new Mock<DbSet<LocJobList>>();

            dbSet.As<IQueryable<LocJobList>>().Setup(m => m.Provider).Returns(queryableLocJobList.Provider);
            dbSet.As<IQueryable<LocJobList>>().Setup(m => m.Expression).Returns(queryableLocJobList.Expression);
            dbSet.As<IQueryable<LocJobList>>().Setup(m => m.ElementType).Returns(queryableLocJobList.ElementType);
            dbSet.As<IQueryable<LocJobList>>().Setup(m => m.GetEnumerator()).Returns(queryableLocJobList.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocJobList>())).Callback<LocJobList>((s) => locJobList.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocJobList>())).Callback<LocJobList>((s) => locJobList.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locJobList.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocJobList> GetLocJobList()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocJobList) + ".csv");
            var items = CsvParser.Parse<LocJobListForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocJobList
                {
                    Id = item.ID,
                    IdisoCoding = item.IDIsoCoding,
                    JobName = item.JobName,
                    UserName = item.UserName
                };
            });

            return items.ToList();
        }
    }
}
