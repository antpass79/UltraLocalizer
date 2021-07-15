using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.LocalizationService.Utilities;

namespace MyLabLocalizer.LocalizationService.Services
{
    public interface ILocalizationResourceBuilder
    {
        ILocalizationResourceBuilder ComponentNamespaceGroup(ComponentNamespaceGroup<ComponentNamespace,InternalNamespace> componentNamespaceGroup);
        ILocalizationResourceBuilder Language(Language language);
        ILocalizationResourceBuilder DebugMode(bool debugMode);

        LocalizationResource Build();
    }
}
