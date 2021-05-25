using Globe.Shared.DTOs;
using Globe.Shared.Utilities;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ConceptService : IAsyncConceptService
    {
        const bool IS_MASTER = true;
 
        private readonly IAsyncNotificationService _notificationService;
        
        private readonly LocalizationContext _localizationContext;
        private readonly UltraDBStrings _ultraDBStrings;
        private readonly UltraDBStrings2Context _ultraDBStrings2Context;
        private readonly UltraDBConcept _ultraDBConcept;
        private readonly IXmlToDBService _xmlToDBService;
        
        public ConceptService(
            IAsyncNotificationService notificationService,
            LocalizationContext localizationContext,
            UltraDBStrings ultraDBStrings,
            UltraDBStrings2Context ultraDBStrings2Context,
            UltraDBConcept ultraDBConcept,
            IXmlToDBService xmlToDBService)
        {
            _notificationService = notificationService;
            _localizationContext = localizationContext;
            _ultraDBStrings = ultraDBStrings;
            _ultraDBStrings2Context = ultraDBStrings2Context;
            _ultraDBConcept = ultraDBConcept;
            _xmlToDBService = xmlToDBService;
        }

        async public Task SaveAsync(SavableConceptModelDTO savableConceptModel)
        {
            if (savableConceptModel.Language.IsoCoding == SharedConstants.LANGUAGE_EN)
            {
                foreach (var context in savableConceptModel.Concept.EditableContexts)
                {
                    // Nothing happens
                    if (context.OldStringId != 0 && context.OldStringId == context.StringId)
                    {
                        continue;
                    }

                    if (context.OldStringId != 0)
                    {
                        var context2Contexts = _ultraDBStrings.GetConcept2ContextStrings(context.Concept2ContextId);
                        foreach (var context2Context in context2Contexts)
                        {
                            _localizationContext.DeletebyIDStringIDConcept2Context(context2Context.IDString, context.Concept2ContextId);
                        }
                    }

                    if (context.StringId == 0)
                    {
                        var stringId = _ultraDBStrings.InsertNewString(1, (int)context.StringType, context.StringEditableValue);
                        _ultraDBStrings2Context.InsertNewStrings2Context(stringId, context.Concept2ContextId);
                        Console.WriteLine($"New string {context.StringEditableValue} has been inserted with type ID = {context.StringType}");
                    }
                    else
                    {
                        //Questa funzione non fa altro che trovare tutte le stringhe che si riferiscono allo stesso conceptToContext, ma hanno diverso language.
                        var dbStrings = _ultraDBStrings.GetConceptContextEquivalentStrings(context.StringId);
                        foreach (var dbString in dbStrings)
                        {
                            _localizationContext.InsertNewStrings2Context(dbString.IDString, context.Concept2ContextId);
                        }
                    }
                }
                if (savableConceptModel.Concept.Id != 0)
                {
                    _ultraDBConcept.UpdateConcept(savableConceptModel.Concept.Id, savableConceptModel.Concept.IgnoreTranslation, savableConceptModel.Concept.MasterTranslatorComment);
                    Console.WriteLine($"Concept {savableConceptModel.Concept.Id} has been updated with the comment: {savableConceptModel.Concept.MasterTranslatorComment}");

                }
            }
            else
            {
                UltraDBExtendedStrings.Languages language = UltraDBExtendedStrings.ParseFromString(savableConceptModel.Language.IsoCoding);
                foreach (var context in savableConceptModel.Concept.EditableContexts)
                {
                    if (context.OldStringId != 0)
                    {
                        _localizationContext.DeletebyIDStringIDConcept2Context(context.OldStringId, context.Concept2ContextId);
                    }

                    var stringId = _ultraDBStrings.InsertNewString((int)language, (int)context.StringType, context.StringEditableValue);
                    _ultraDBStrings2Context.InsertNewStrings2Context(stringId, context.Concept2ContextId);
                    Console.WriteLine($"New string {context.StringEditableValue} has been inserted with type ID = {context.StringType}");
                }
 
                if (savableConceptModel.Concept.Id != 0 && IS_MASTER)
                {
                    _ultraDBConcept.UpdateConcept(savableConceptModel.Concept.Id, savableConceptModel.Concept.IgnoreTranslation, savableConceptModel.Concept.MasterTranslatorComment);
                    Console.WriteLine($"Concept {savableConceptModel.Concept.Id} has been updated with the comment: {savableConceptModel.Concept.MasterTranslatorComment}");
                }
            }

            await _localizationContext.SaveChangesAsync();
        }

        async public Task<NewConceptsResult> CheckNewConceptsAsync()
        {
            //_xmlService.LoadXml();
            //_xmlService.FillDB();
            var statistics = await _xmlToDBService.UpdateDatabaseAsync();

            var result = new NewConceptsResult
            {
                IsNotified = true, 
                Statistics = statistics
            };
            if(statistics.ChangesFound)
            {
                result.IsNotified = false;
                await _notificationService.ConceptsChanged(result);
            }
           
            return await Task.FromResult(result);
        }

        List<DBStrings> GetConcept2ContextStrings_NEW(int concept2ContextId)
        {
            return _localizationContext.VStringsToContexts
                .Where(item => item.Id == concept2ContextId)
                .Select(item => new DBStrings
                {
                    //IDLanguage = item.
                    IDString = item.StringId,
                    //IDString2Context = item.Id,
                    DataString = item.String
                    //IDType = item.str
                })
                .ToList();
        }

        void DeletebyIDStringIDConcept2Context_NEW(int idString, int idConcept2Context)
        {
            var itemToRemove = _localizationContext.LocStrings2Contexts
                .AsQueryable()
                .Where(item => item.Idstring == idString && item.Idconcept2Context == idConcept2Context)
                .Single();
            _localizationContext.LocStrings2Contexts.Remove(itemToRemove);
        }

        int InsertNewString_NEW(int IDLanguage, int IDType, string DataString)
        {
            var item = new LocString
            {
                Idlanguage = IDLanguage,
                Idtype = IDType,
                String = DataString
            };

            _localizationContext.LocStrings.Add(item);
            _localizationContext.SaveChanges();
            //TODO. Ritorno id inserita
            return item.Id;
        }

        void InsertNewStrings2Context_NEW(int IDString, int IDConcept2Context)
        {
            _localizationContext.LocStrings2Contexts.Add(new LocStrings2Context
            {
                Idstring = IDString,
                Idconcept2Context = IDConcept2Context
            });
        }

        List<DBStrings> GetConceptContextEquivalentStrings_NEW(int StringId)
        {
            var calculatedStringToContextId = _localizationContext.LocStrings2Contexts
                .AsQueryable()
                .Where(item => item.Idstring == StringId)
                .Select(item => item.Idconcept2Context)
                .Single();

            var results = _localizationContext.VStrings
                .AsQueryable()
                .Where(item => item.ConceptToContextId == calculatedStringToContextId)
                .Select(item => new DBStrings
                {
                    DataString = item.String,
                    IDLanguage = item.LanguageId,
                    IDString = item.Id,
                    IDType = item.StringTypeId,
                    IDString2Context = item.StringToContextId
                })
                .ToList();

            return results;
        }

        void UpdateConcept_NEW(int Id, bool Ignore, string Comment)
        {
            var concept = _localizationContext.LocConceptsTables.Find(Id);
            concept.Ignore = Ignore;
            concept.Comment = Comment;
        }
    }
}
