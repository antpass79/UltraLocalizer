using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class GroupedStringEntityAdapterService : IAsyncGroupedStringEntityService
    {
        private readonly UltraDBEditConcept _ultraDBEditConcept;

        public GroupedStringEntityAdapterService(UltraDBEditConcept ultraDBEditConcept)
        {
            _ultraDBEditConcept = ultraDBEditConcept;
        }

        async public Task<IEnumerable<GroupedStringEntity>> GetAllAsync(string componentNamespace, string InternalNamespace, string ISOCoding, int jobListId)
        {
            return await Task.FromResult(_ultraDBEditConcept.GetGroupledDataBy(componentNamespace, InternalNamespace, ISOCoding, jobListId));
        }

        public Task<IEnumerable<GroupedStringEntity>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<GroupedStringEntity> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
