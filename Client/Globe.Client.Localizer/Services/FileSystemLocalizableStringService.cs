using Globe.Client.Localizer.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Globe.Client.Localizer.Services
{
    public interface IFileSystemLocalizableStringService : IAsyncLocalizableStringService
    {
    }

    public class FileSystemLocalizableStringService : IFileSystemLocalizableStringService
    {
        readonly string FILE_JSON = "file_json.txt";

        public FileSystemLocalizableStringService()
        {
        }

        async public Task<IEnumerable<LocalizableString>> GetAllAsync()
        {
            string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), FILE_JSON));
            IEnumerable<LocalizableString> strings = JsonConvert.DeserializeObject<IEnumerable<LocalizableString>>(json);

            return await Task.FromResult(strings);
        }

        async public Task SaveAsync(IEnumerable<LocalizableString> strings)
        {
            string json = JsonConvert.SerializeObject(strings);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), FILE_JSON), json);

            await Task.CompletedTask;
        }
    }
}
