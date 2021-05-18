using Globe.Shared.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;

namespace Globe.TranslationServer.Services
{
    public interface ILocalizationResourceBuilder
    {
        ILocalizationResourceBuilder ComponentNamespaceGroup(ComponentNamespaceGroup<ComponentNamespace,InternalNamespace> componentNamespaceGroup);
        ILocalizationResourceBuilder Language(Language language);
        ILocalizationResourceBuilder DebugMode(bool debugMode);

        LocalizationResource Build();
    }
}
