﻿using Globe.TranslationServer.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public interface IXmlToDbMergeService
    {
        Task<IEnumerable<MergiableConcept>> MergeFilteredEntriesIntoDatabaseAsync(Dictionary<string, LocalizationSectionSubset> keyLocalizationSectionSubsetPairs);
    }
}