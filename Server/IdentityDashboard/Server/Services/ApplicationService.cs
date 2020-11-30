using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public partial class ApplicationService : IAsyncApplicationService
    {
        #region Public Functions

        public Stream Zip()
        {
            string dirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "UltraLocalizer");
            List<InMemoryFile> files = new List<InMemoryFile>();

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
