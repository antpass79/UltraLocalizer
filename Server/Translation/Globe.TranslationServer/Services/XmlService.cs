﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace Globe.TranslationServer.Services
{
    public class XmlService : IAsyncXmlService
    {
        #region Data Members

        private readonly IDBToXmlService _dbToXmlService;

        #endregion

        #region Constructors

        public XmlService(IDBToXmlService dbToXmlService)
        {
            _dbToXmlService = dbToXmlService;
        }

        #endregion

        #region Public Functions

        public Stream Zip()
        {
            string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Zip");

            CleanOutputFolder(outputFolder);
            GenerateXmlFiles(outputFolder);
            var inMemoryFiles = LoadInMemoryFiles(outputFolder);

            return Zip(inMemoryFiles);
        }

        #endregion

        #region Private Functions

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
                Directory.CreateDirectory(outputFolder);
            }
            catch(Exception e)
            {
                throw new InvalidOperationException($"Error during {nameof(CleanOutputFolder)}", e);
            }
        }

        private void GenerateXmlFiles(string outputFolder)
        {
            try
            {
                _dbToXmlService.Generate(outputFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                throw new InvalidOperationException($"Error during {nameof(Zip)}", e);
            }
        }

        #endregion
    }
}
