using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Tests.Csv
{
    static class CsvParser
    {
        static public IEnumerable<T> Parse<T>(string csvFile)
        {
            using (var streamReader = new StreamReader(csvFile))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                ConfigureCsvParser(csvReader);
                return csvReader.GetRecords<T>().ToList();
            }
        }

        static void ConfigureCsvParser(CsvReader csvReader)
        {
            csvReader.Configuration.Delimiter = "|";
            csvReader.Configuration.MissingFieldFound = (headerNames, index, context) =>
            {
                if (headerNames == null)
                    return;

                Console.WriteLine(string.Format("Field with names ['{0}'] at index '{1}' was not found.", string.Join("', '", headerNames), index));
            };
            csvReader.Configuration.ReadingExceptionOccurred = exception =>
            {
                Console.WriteLine($"Bad data found on row '{exception.Message}'");
                return false;
            };
            csvReader.Configuration.BadDataFound = context =>
            {
                Console.WriteLine(string.Format("Bad data found {0}.", context.RawRecord));
            };
        }
    }
}
