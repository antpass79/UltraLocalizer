using Globe.Shared.DTOs;
using Globe.Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace Globe.TranslationServer.Services
{
    public class XmlZipService : IAsyncXmlZipService
    {
        #region Data Members

        private readonly IDBToXmlService _dbToXmlService;
        private readonly ILogService _logService;

        private static object _lock = new object();

        #endregion

        #region Constructors

        public XmlZipService(
            IDBToXmlService dbToXmlService,
            ILogService logService)
        {
            _dbToXmlService = dbToXmlService;
            _logService = logService;
        }

        #endregion

        #region Public Functions

        public Stream Zip(ExportDbFilters exportDbFilters)
        {
            lock (_lock)
            {
                string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Zip");
                outputFolder = Path.Combine(outputFolder, Guid.NewGuid().ToString());
                _logService.Info($"XmlZipService.Zip - Output folder {outputFolder}");

                CreateOutputFolder(outputFolder);
                GenerateXmlFiles(outputFolder, exportDbFilters);
                var inMemoryFiles = LoadInMemoryFiles(outputFolder);
                CleanOutputFolder(outputFolder);
                
                return Zip(inMemoryFiles);
            }
        }

        #endregion

        #region Private Functions
        private void CreateOutputFolder(string outputFolder)
        {
            try
            {
                CleanOutputFolder(outputFolder);
                Directory.CreateDirectory(outputFolder);
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                throw new InvalidOperationException($"Error during {nameof(CreateOutputFolder)}", e);
            }
        }

        private void CleanOutputFolder(string outputFolder)
        {
            try
            {
                if (Directory.Exists(outputFolder))
                {
                    var files = Directory.GetFiles(outputFolder);
                    files
                        .ToList()
                        .ForEach(file => File.Delete(file));
                    Directory.Delete(outputFolder);
                }
            }
            catch(Exception e)
            {
                _logService.Exception(e);
                throw new InvalidOperationException($"Error during {nameof(CleanOutputFolder)}", e);
            }
        }

        private void GenerateXmlFiles(string outputFolder, ExportDbFilters exportDbFilters)
        {
            try
            {
                _dbToXmlService.Generate(outputFolder, exportDbFilters);
            }
            catch (AggregateException e)
            {
                _logService.Exception(e);
                throw new InvalidOperationException($"Error during {nameof(GenerateXmlFiles)}", e);
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                throw new InvalidOperationException($"Error during {nameof(GenerateXmlFiles)}", e);
            }
        }

        private IEnumerable<InMemoryFile> LoadInMemoryFiles(string outputFolder)
        {
            try
            {
                var inMemoryFiles = new List<InMemoryFile>();
                string[] fileNames = Directory.GetFiles(outputFolder);
                foreach (var fileName in fileNames)
                {
                    inMemoryFiles.Add(new InMemoryFile { FileName = Path.GetFileName(fileName), Content = File.ReadAllBytes(fileName) });
                }

                return inMemoryFiles;
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                throw new InvalidOperationException($"Error during {nameof(LoadInMemoryFiles)}", e);
            }
        }

        private Stream Zip(IEnumerable<InMemoryFile> inMemoryFiles)
        {
            try
            {
                var zipStream = new MemoryStream();

                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in inMemoryFiles)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                        using (var zipArchive = zipArchiveEntry.Open())
                            zipArchive.Write(file.Content, 0, file.Content.Length);
                    }
                }

                return zipStream;
            }
            catch (Exception e)
            {
                _logService.Exception(e);
                throw new InvalidOperationException($"Error during {nameof(Zip)}", e);
            }
        }

        #endregion
    }
}
