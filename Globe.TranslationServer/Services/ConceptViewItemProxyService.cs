using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class ConceptViewItemProxyService : IAsyncConceptViewItemProxyService
    {
        private readonly UltraDBConcept _ultraDBConcept;

        public ConceptViewItemProxyService(UltraDBConcept ultraDBConcept)
        {
            _ultraDBConcept = ultraDBConcept;
        }

        async public Task<IEnumerable<ConceptViewItemDTO>> GetAllAsync(ConceptViewItemSearchDTO search)
        {
            IEnumerable<DBConceptSearch> items;

            if (search.SearchBy == ConceptSearchBy.Concept)
            {
                items = search.FilterBy switch
                {
                    ConceptFilterBy.None => _ultraDBConcept.GetSearchConceptbyISO(search.ISOCoding, search.StringValue, true),
                    ConceptFilterBy.Context => _ultraDBConcept.GetSearchConceptbyISObyContext(search.ISOCoding, search.StringValue, search.Context, true),
                    ConceptFilterBy.StringType => _ultraDBConcept.GetSearchConceptbyISObyStringType(search.ISOCoding, search.StringValue, search.StringValue, true),
                    _ => new List<DBConceptSearch>()
                };
            }
            else
            {
                items = search.FilterBy switch
                {
                    ConceptFilterBy.None => _ultraDBConcept.GetSearchStringbyISO(search.ISOCoding, search.StringValue, true),
                    ConceptFilterBy.Context => _ultraDBConcept.GetSearchStringtbyISObyContext(search.ISOCoding, search.StringValue, search.Context, true),
                    ConceptFilterBy.StringType => _ultraDBConcept.GetSearchStringtbyISObyStringType(search.ISOCoding, search.StringValue, search.StringValue, true),
                    _ => new List<DBConceptSearch>()
                };
            }

            //// aggiungiamo i commenti SW
            //foreach (DBConceptSearch item in lDb)
            //{
            //    string key = _manager.GetKey(item.ComponentNamespace, item.InternalNamespace, item.LocalizationID);
            //    try
            //    {
            //        string Comment = _manager.KeyComments[key];
            //        item.SWComment = Comment;
            //    }
            //    catch (System.Exception)
            //    {

            //    }
            //}

            var result = items.Select(item =>
            {
                return new ConceptViewItemDTO
                {
                    ComponentNamespace = item.ComponentNamespace,
                    Concept = item.LocalizationID,
                    Context = item.Context,
                    InternalNamespace = item.InternalNamespace,
                    Value = item.String,
                    SoftwareComment = item.SWComment,
                    MasterTranslatorComment = item.MTComment
                };
            });

            return await Task.FromResult(result);
        }
    }
}
