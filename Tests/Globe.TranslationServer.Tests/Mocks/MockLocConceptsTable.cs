using MyLabLocalizer.LocalizationService.Entities;
using MyLabLocalizer.LocalizationService.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyLabLocalizer.LocalizationService.Tests.Mocks
{
    class MockLocConceptsTable
    {
        public Mock<DbSet<LocConceptsTable>> Mock()
        {
            var locConceptsTables = GetLocConceptsTable();
            var queryableLocConceptsTables = locConceptsTables.AsQueryable();
            var dbSet = new Mock<DbSet<LocConceptsTable>>();

            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.Provider).Returns(queryableLocConceptsTables.Provider);
            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.Expression).Returns(queryableLocConceptsTables.Expression);
            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.ElementType).Returns(queryableLocConceptsTables.ElementType);
            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.GetEnumerator()).Returns(queryableLocConceptsTables.GetEnumerator());

            dbSet.Setup(m => m.Add(It.IsAny<LocConceptsTable>())).Callback<LocConceptsTable>((s) => locConceptsTables.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<LocConceptsTable>())).Callback<LocConceptsTable>((s) => locConceptsTables.Remove(s));
            dbSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>((@params) => locConceptsTables.Find(item => item.Id == (int)@params[0]));

            return dbSet;
        }

        private List<LocConceptsTable> GetLocConceptsTable()
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string csvFile = Path.Combine(directory, nameof(LocConceptsTable) + ".csv");
            var items = CsvParser.Parse<LocConceptsTableForCsv>(csvFile).ToList().Select(item =>
            {
                return new LocConceptsTable
                {
                    Id = item.ID,
                    ComponentNamespace = item.ComponentNamespace,
                    InternalNamespace = item.InternalNamespace,
                    LocalizationId = item.LocalizationID,
                    Ignore = item.Ignore,
                    Comment = item.Comment,
                    SsmaTimeStamp = item.SSMA_TimeStamp
                };
            });

            return items.ToList();
        }
    }
}
