using Globe.Client.Localizer.Models;
using System.Collections.Generic;

namespace Globe.Client.Localizer.Services
{
    public interface IStringMergeService
    {
        IEnumerable<LocalizableString> Merge(IEnumerable<LocalizableString> source1, IEnumerable<LocalizableString> source2);
        bool Equal(IEnumerable<LocalizableString> source1, IEnumerable<LocalizableString> source2);
    }
}
