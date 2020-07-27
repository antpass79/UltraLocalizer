using Globe.Client.Localizer.Models;

namespace Globe.Client.Localizer.Extensions
{
    public static class LocalizableStringExtensions
    {
        static public LocalizableString Clone(this LocalizableString localizableString)
        {
            return new LocalizableString
            {
                Id = localizableString.Id,
                Key = localizableString.Key,
                Language = localizableString.Language,
                SavedOn = localizableString.SavedOn,
                Value = localizableString.Value
            };
        }
    }
}
