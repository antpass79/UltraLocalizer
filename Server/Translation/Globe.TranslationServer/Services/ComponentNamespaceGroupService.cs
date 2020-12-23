using Globe.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ComponentNamespaceGroupService : IComponentNamespaceGroupService
    {
        private readonly IUltraDBJobGlobal _ultraDBJobGlobal;

        public ComponentNamespaceGroupService(IUltraDBJobGlobal ultraDBJobGlobal)
        {
            _ultraDBJobGlobal = ultraDBJobGlobal;
        }

        async public Task<IEnumerable<ComponentNamespaceGroup>> GetAllAsync(Language language)
        {
            var result = _ultraDBJobGlobal
                .GetMissingDataBy(language.IsoCoding);

            return await Task.FromResult(result.Select(group => new ComponentNamespaceGroup
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
