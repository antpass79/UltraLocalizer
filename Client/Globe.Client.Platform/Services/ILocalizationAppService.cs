using Globe.Client.Platform.Utilities;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface ILocalizationAppService
    {
        string SelectedLanguage { get; }
        Task<LocalizedDictionary> LoadAsync(string language);
        string Resolve(string key);
    }
}
