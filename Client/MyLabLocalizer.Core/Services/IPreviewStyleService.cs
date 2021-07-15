using MyLabLocalizer.Core.Models;

namespace MyLabLocalizer.Core.Services
{
    public interface IPreviewStyleService
    {
        PreviewStyleInfo this[string contextName] { get; }
        PreviewStyleInfo this[string typeName, string contextName] { get; }
    }
}
