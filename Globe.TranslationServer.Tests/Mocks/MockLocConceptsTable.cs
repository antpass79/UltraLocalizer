using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Tests.Csv;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Mocks
{
    class MockLocConceptsTable
    {
        public Mock<DbSet<LocConceptsTable>> Mock()
        {
            var locConceptsTables = GetLocConceptsTable().AsQueryable();
            var dbSet = new Mock<DbSet<LocConceptsTable>>();

            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.Provider).Returns(locConceptsTables.Provider);
            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.Expression).Returns(locConceptsTables.Expression);
            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.ElementType).Returns(locConceptsTables.ElementType);
            dbSet.As<IQueryable<LocConceptsTable>>().Setup(m => m.GetEnumerator()).Returns(locConceptsTables.GetEnumerator());

            return dbSet;
        }

        private IEnumerable<LocConceptsTable> GetLocConceptsTable()
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

            return items;
        }
    }
}
