using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class StyleService : IAsyncStyleService
    {
        public async Task<string> GetFileContent(string filePath)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return await Task.FromResult(string.Empty);
            }
            
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), $"Styles/{filePath}");

            using (var reader = new StreamReader(path))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
