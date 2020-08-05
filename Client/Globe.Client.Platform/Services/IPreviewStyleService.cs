using Globe.Client.Platform.Models;

namespace Globe.Client.Platform.Services
{
    public interface IPreviewStyleService
    {
        PreviewStyleInfo this[string contextName] { get; }
        PreviewStyleInfo this[string typeName, string contextName] { get; }
    }
}
