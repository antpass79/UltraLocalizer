using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;

namespace Globe.TranslationServer.Services
{
    public interface ILocalizationResourceBuilder
    {
        ILocalizationResourceBuilder Component(VLocalization component);
        ILocalizationResourceBuilder Language(LocLanguage language);
        ILocalizationResourceBuilder DebugMode(bool debugMode);

        LocalizationResource Build();
    }
}
