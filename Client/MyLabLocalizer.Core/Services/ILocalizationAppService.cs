using MyLabLocalizer.Core.Utilities;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
{
    public interface ILocalizationAppService
    {
        string SelectedLanguage { get; }
        Task<LocalizedDictionary> LoadAsync(string language);
        string Resolve(string key);
    }
}
