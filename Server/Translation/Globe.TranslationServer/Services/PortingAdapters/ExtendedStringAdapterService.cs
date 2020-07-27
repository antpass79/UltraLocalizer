using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ExtendedStringAdapterService : IAsyncExtendedStringService
    {
        private readonly UltraDBEditConcept _ultraDBEditConcept;

        public ExtendedStringAdapterService(UltraDBEditConcept ultraDBEditConcept)
        {
            _ultraDBEditConcept = ultraDBEditConcept;
        }

        public Task<IEnumerable<DBExtendedStrings>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        async public Task<IEnumerable<DBExtendedStrings>> GetAllAsync(string componentName, string internalNamespace, string ISOCoding, int jobListId, int conceptId)
        {
            var groupedStrings = _ultraDBEditConcept.GetGroupledDataBy(componentName, internalNamespace, ISOCoding, jobListId);
            var concept = (from groupedString in groupedStrings
                           where groupedString.ConceptID == conceptId
                           select new DBConcept { ComponentNamespace = groupedString.ComponentNamespace, LocalizationID = groupedString.LocalizationID, InternalNamespace = groupedString.InternalNamespace, IDConcept = groupedString.ConceptID }).FirstOrDefault();

            var result = (from ui in groupedStrings
                          where ui.ConceptID == conceptId
                                  from s in ui.Group
                                  select new DBExtendedStrings
                                  {
                                      ContextName = s.ContextName,
                                      IDConcept2Context = s.IDConcept2Context,
                                      StringType = s.StringType,
                                      IDTYpe = s.StringTypeID,
                                      Is2Translate = false,
                                      IsLocked = false,
                                      DataStringENG = s.StringENG,
                                      DataString = s.DataString,
                                      ISOCoding = ISOCoding,
                                      IDString = s.IDString,
                                      OldIDString = s.IDString,
                                      OriginalString = s.DataString
                                  }).Distinct().ToList();

            return await Task.FromResult(result);
        }

        public Task<DBExtendedStrings> GetAsync(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
