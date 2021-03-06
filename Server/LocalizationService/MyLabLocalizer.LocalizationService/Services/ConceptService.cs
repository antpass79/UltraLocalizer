﻿using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.DTOs;
using MyLabLocalizer.LocalizationService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class ConceptService : IAsyncConceptService
    {
        const bool IS_MASTER = true;

        private readonly LocalizationContext _localizationContext;
        private readonly IAsyncNotificationService _notificationService;
        private readonly IXmlToDBService _xmlToDBService;

        public ConceptService(
            LocalizationContext localizationContext,
            IAsyncNotificationService notificationService,
            IXmlToDBService xmlToDBService)
        {
            _localizationContext = localizationContext;
            _notificationService = notificationService;
            _xmlToDBService = xmlToDBService;
        }

        async public Task<ConceptDTO> GetConceptAsync(int id)
        {
            var entity = await _localizationContext.LocConceptsTables.FindAsync(id);
            return new ConceptDTO
            {
                Id = entity.Id,
                Value = entity.LocalizationId,
                ComponentNamespace = entity.ComponentNamespace,
                InternalNamespace = entity.InternalNamespace,
                Comment = entity.Comment,
                Ignore = entity.Ignore.HasValue && entity.Ignore.Value
            };
        }

        async public Task UpdateAsync(TranslatedString translatedString)
        {
            var localizeString = _localizationContext.LocStrings.Find(translatedString.Id);

            localizeString.String = translatedString.Value;
            
            await _localizationContext.SaveChangesAsync();
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
                        //Attenzione la funzione sotto deve tornare assolutamente una List e non un IEnumerable, altrimenti viene sollevata eccezione dal datareader che non viene "chiuso"
                        var stringIds = GetConcept2ContextStrings(context.Concept2ContextId);
                        foreach (var stringId in stringIds)
                        {
                            DeletebyIDStringIDConcept2Context(stringId, context.Concept2ContextId);
                        }
                    }

                    if (context.StringId == 0)
                    {
                        InsertNewStringAndNewStringToContext(1, (int)context.StringType, context.StringEditableValue, context.Concept2ContextId);
                        Console.WriteLine($"New string {context.StringEditableValue} has been inserted with type ID = {context.StringType}");
                    }
                    else
                    {
                        //Questa funzione non fa altro che trovare tutte le stringhe che si riferiscono allo stesso conceptToContext, ma hanno diverso language.
                        var stringIds = GetConceptContextEquivalentStrings(context.StringId);
                        foreach (var stringId in stringIds)
                        {
                            InsertNewStrings2Context(stringId, context.Concept2ContextId);
                        }
                    }
                }
                if (savableConceptModel.Concept.Id != 0)
                {
                    UpdateConcept(savableConceptModel.Concept.Id, savableConceptModel.Concept.IgnoreTranslation, savableConceptModel.Concept.MasterTranslatorComment);
                    Console.WriteLine($"Concept {savableConceptModel.Concept.Id} has been updated with the comment: {savableConceptModel.Concept.MasterTranslatorComment}");
                }
            }
            else
            {
                foreach (var context in savableConceptModel.Concept.EditableContexts)
                {
                    if (context.OldStringId != 0)
                    {
                        DeletebyIDStringIDConcept2Context(context.OldStringId, context.Concept2ContextId);
                    }

                    InsertNewStringAndNewStringToContext(savableConceptModel.Language.Id, (int)context.StringType, context.StringEditableValue, context.Concept2ContextId);
                    Console.WriteLine($"New string {context.StringEditableValue} has been inserted with type ID = {context.StringType}");
                }

                if (savableConceptModel.Concept.Id != 0 && IS_MASTER)
                {
                    UpdateConcept(savableConceptModel.Concept.Id, savableConceptModel.Concept.IgnoreTranslation, savableConceptModel.Concept.MasterTranslatorComment);
                    Console.WriteLine($"Concept {savableConceptModel.Concept.Id} has been updated with the comment: {savableConceptModel.Concept.MasterTranslatorComment}");
                }
            }

            await _localizationContext.SaveChangesAsync();
        }

        async public Task<NewConceptsResult> CheckNewConceptsAsync()
        {
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

        List<int> GetConcept2ContextStrings(int concept2ContextId)
        {
            return _localizationContext.VStringsToContexts
                .Where(item => item.Id == concept2ContextId)
                .Select(item => item.StringId)
                .ToList();
        }

        void DeletebyIDStringIDConcept2Context(int idString, int idConcept2Context)
        {
            var itemToRemove = _localizationContext.LocStrings2Contexts
                .Where(item => item.Idstring == idString && item.Idconcept2Context == idConcept2Context)
                .Single();
            _localizationContext.LocStrings2Contexts.Remove(itemToRemove);
        }

        void InsertNewStringAndNewStringToContext(int idLanguage, int idType, string dataString, int conceptToContextId)
        {
            var newString = new LocString
            {
                Idlanguage = idLanguage,
                Idtype = idType,
                String = dataString
            };

            _localizationContext.LocStrings.Add(newString);

            var newStringToContext = new LocStrings2Context
            {
                Idconcept2Context = conceptToContextId
            };

            newString.LocStrings2Contexts.Add(newStringToContext);
        }

        void InsertNewStrings2Context(int idString, int idConcept2Context)
        {
            _localizationContext.LocStrings2Contexts.Add(new LocStrings2Context
            {
                Idstring = idString,
                Idconcept2Context = idConcept2Context
            });
        }

        List<int> GetConceptContextEquivalentStrings(int stringId)
        {
            var calculatedStringToContextId = _localizationContext.LocStrings2Contexts
                .Where(item => item.Idstring == stringId)
                .Select(item => item.Idconcept2Context)
                .Single();

            var results = _localizationContext.VStrings
                .Where(item => item.ConceptToContextId == calculatedStringToContextId)
                .Select(item => item.Id)
                .ToList();

            return results;
        }

        void UpdateConcept(int id, bool ignore, string comment)
        {
            var concept = _localizationContext.LocConceptsTables.Find(id);
            concept.Ignore = ignore;
            concept.Comment = comment;
        }
    }
}
