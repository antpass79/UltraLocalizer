using AutoMapper;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class InternalNamespaceGroupService : IAsyncInternalNamespaceGroupService
    {
        private readonly IUltraDBJobGlobal _ultraDBJobGlobal;

        public InternalNamespaceGroupService(IUltraDBJobGlobal ultraDBJobGlobal)
        {
            _ultraDBJobGlobal = ultraDBJobGlobal;
        }

        async public Task<IEnumerable<InternalNamespaceGroupDTO>> GetAllAsync(LanguageDTO language)
        {
            var result = _ultraDBJobGlobal
                .GetMissingDataBy(language.IsoCoding);

            return await Task.FromResult(result.Select(group => new InternalNamespaceGroupDTO
            {
                ComponentNamespace = new ComponentNamespaceDTO { Description = group.ComponentNamespace },
                InternalNamespaces = group.InternalName.Select(item => new InternalNamespaceDTO
                {
                    Description = item.InternalNamespace
                })
            }));
        }
    }
}
