using MyLabLocalizer.Models;
using System.Collections.Generic;

namespace MyLabLocalizer.Services
{
    public interface IStringMergeService
    {
        IEnumerable<LocalizableString> Merge(IEnumerable<LocalizableString> source1, IEnumerable<LocalizableString> source2);
        bool Equal(IEnumerable<LocalizableString> source1, IEnumerable<LocalizableString> source2);
    }
}
