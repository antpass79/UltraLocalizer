using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.XmlGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Globe.TranslationServer.Services
{
    public class XmlService : IAsyncXmlService
    {
        #region Data Members

        private readonly LocalizationContext _context;

        #endregion

        #region Constructors

        public XmlService(LocalizationContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Functions

        public Stream Zip()
        {
            string dirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Zip");
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            List<InMemoryFile> files = new List<InMemoryFile>();
            Convertion conv = new Convertion(dirPath, _context);
            conv.EraseOldFiles();

            //Con questa funzione avremo n file xml in filePath (che dovrebbe essere dirPath) uno per la coppia language/ComponentNameSpace
            if (!conv.LocalizeDB(true))
            {
                throw new NotSupportedException();
            }

            string[] fileNames = Directory.GetFiles(dirPath);
            foreach (var fileName in fileNames)
            {
                files.Add(new InMemoryFile { FileName = Path.GetFileName(fileName), Content = File.ReadAllBytes(fileName) });
            }

            var zipStream = new MemoryStream();

            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                    using (var zipArchive = zipArchiveEntry.Open())
                        zipArchive.Write(file.Content, 0, file.Content.Length);
                }
            }

            return zipStream;
        }

        #endregion
    }
}
