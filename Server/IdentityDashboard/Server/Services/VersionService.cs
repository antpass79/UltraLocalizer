using Globe.Identity.AdministrativeDashboard.Server.DTOs;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class VersionService : IAsyncVersionService
    {
        public async Task<VersionDTO> Get()
        {
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var content = await File.ReadAllTextAsync(Path.Combine(directory, "version.json"));
            var version = JsonConvert.DeserializeObject<VersionDTO>(content);

            return version;
        }
    }
}
