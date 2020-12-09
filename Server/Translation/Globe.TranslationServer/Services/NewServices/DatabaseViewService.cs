//using Globe.TranslationServer.Entities;
//using Globe.TranslationServer.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

//namespace Globe.TranslationServer.Services.NewServices
//{
//    public class DatabaseViewService : IDatabaseViewService
//    {
//        private readonly LocalizationContext _context;

//        public DatabaseViewService(LocalizationContext context)
//        {
//            _context = context;
//        }

//        public IEnumerable<DatabaseView> Get(Expression<Func<DatabaseView, bool>> filter = null)
//        {
//            var view = GetView().ToList();
//            if (filter != null)
//                view = view
//                    .AsQueryable()
//                    .Where(filter)
//                    .ToList();

//            return view;
//        }

//        private IEnumerable<DatabaseView> GetView()
//        {
//            var result = from concept in _context.LocConceptsTable

//                         join concept2Context in _context.LocConcept2Context
//                         on concept.Id equals concept2Context.Idconcept

//                         join string2Context in _context.LocStrings2Context
//                         on concept2Context.Id equals string2Context.Idconcept2Context

//                         join @string in _context.LocStrings
//                         on string2Context.Idstring equals @string.Id

//                         join language in _context.LocLanguages
//                         on @string.Idlanguage equals language.Id

//                         join context in _context.LocContexts
//                         on concept2Context.Idcontext equals context.Id

//                         //join stringAcceptable in _context.LocStringsacceptable
//                         //on @string.Id equals stringAcceptable.IdString into data
//                         //from subStringAcceptable in data.DefaultIfEmpty()

//                         select new DatabaseView
//                         {
//                             ComponentNamespace = concept.ComponentNamespace,
//                             InternalNamespace = concept.InternalNamespace,
//                             LocalizationId = concept.LocalizationId,
//                             ContextName = context.ContextName,
//                             StringId = @string.Id,
//                             DataString = @string.String,
//                             //IsAcceptable = data != null
//                         };

//            return result;
//        }
//    }
//}
