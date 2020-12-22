using Globe.Shared.DTOs;
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

        async public Task<IEnumerable<InternalNamespaceGroup>> GetAllAsync(Language language)
        {
            var result = _ultraDBJobGlobal
                .GetMissingDataBy(language.IsoCoding);

            return await Task.FromResult(result.Select(group => new InternalNamespaceGroup
            {
                ComponentNamespace = new ComponentNamespace { Description = group.ComponentNamespace },
                InternalNamespaces = group.InternalName.Select(item => new InternalNamespace
                {
                    Description = item.InternalNamespace
                })
            }));
        }
    }
}
