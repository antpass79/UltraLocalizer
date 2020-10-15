using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface ILocalizationAppService
    {
        string SelectedLanguage { get; }
        Task<Dictionary<string, string>> LoadAsync(string language);
        string Resolve(string key);
    }
}
