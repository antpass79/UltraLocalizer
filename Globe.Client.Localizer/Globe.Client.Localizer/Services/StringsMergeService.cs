using Globe.Client.Localizer.Extensions;
using Globe.Client.Localizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.Client.Localizer.Services
{
    public class StringsMergeService : IStringMergeService
    {
        public IEnumerable<LocalizableString> Merge(IEnumerable<LocalizableString> source1, IEnumerable<LocalizableString> source2)
        {
            if (source1 == null)
                source1 = new List<LocalizableString>();
            if (source2 == null)
                source2 = new List<LocalizableString>();

            var mergedStrings = new List<LocalizableString>();

            source1 = source1.OrderBy(item => item.Key);
            source2 = source2.OrderBy(item => item.Key);

            var count = Math.Max(source1.Count(), source2.Count());

            for (int i = 0; i < count; i++)
            {
                var sourceItem1 = source1.Count() > i ? source1.ElementAt(i) : null;
                var sourceItem2 = source2.Count() > i ? source2.ElementAt(i) : null;

                LocalizableString mergedItem = null;

                if (sourceItem1 == null)
                    mergedItem = sourceItem2;
                else if (sourceItem2 == null)
                    mergedItem = sourceItem1;
                else
                {
                    if (sourceItem1.SavedOn == sourceItem2.SavedOn)
                    {
                        mergedItem = new LocalizableString
                        {
                            Key = "NOT_MERGED",
                            Value = "NOT MERGED"
                        };
                    }
                    else
                        mergedItem = sourceItem1.SavedOn > sourceItem2.SavedOn ? sourceItem1 : sourceItem2;
                }

                mergedStrings.Add(mergedItem.Clone());
            }

            return mergedStrings;
        }

        public bool Equal(IEnumerable<LocalizableString> source1, IEnumerable<LocalizableString> source2)
        {
            if (
                (source1 == null && source2 != null) ||
                (source1 != null && source2 == null) ||
                (source1.Count() != source2.Count()))
                return false;

            source1 = source1.OrderBy(item => item.Key);
            source2 = source2.OrderBy(item => item.Key);

            bool equal = true;

            var count = source1.Count();
            for (var i = 0; i < count; i++)
            {
                var sourceItem1 = source1.ElementAt(i);
                var sourceItem2 = source2.ElementAt(i);

                if (sourceItem1.Key == sourceItem2.Key && sourceItem1.Value == sourceItem2.Value)
                    continue;

                equal = false;
                break;
            }

            return equal;
        }
    }
}
