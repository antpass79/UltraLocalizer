using Globe.Client.Platform.Models;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class LocalVersionService : IVersionService
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
