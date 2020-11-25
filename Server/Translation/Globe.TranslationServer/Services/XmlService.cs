using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.XmlGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

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

        public async Task<byte[]> GetZippedContent()
        {
            string dirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Zip");
            byte[] archiveFile;
            List<InMemoryFile> files = new List<InMemoryFile>();
            Convertion conv = new Convertion(dirPath, _context);
            conv.EraseOldFiles();

            //Con questa funzione avremo n file xml in filePath (che dovrebbe essere dirPath) uno per la coppia language/ComponentNameSpace
            if (!conv.LocalizeDB(true))
            {
                throw new NotSupportedException();
            }

            string[] fileNames = Directory.GetFiles(dirPath);
            foreach( var fileName in fileNames)
            {
                files.Add(new InMemoryFile { FileName = fileName, Content = File.ReadAllBytes(fileName) });
            }

            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(file.Content, 0, file.Content.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return await Task.FromResult(archiveFile);
        }

        #endregion

        public class InMemoryFile
        {
            public string FileName { get; set; }
            public byte[] Content { get; set; }
        }
    }
}
