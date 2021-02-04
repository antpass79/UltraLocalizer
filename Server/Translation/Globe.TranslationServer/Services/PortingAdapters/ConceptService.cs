using DocumentFormat.OpenXml.InkML;
using Globe.Shared.DTOs;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using Globe.TranslationServer.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ConceptService : IAsyncConceptService
    {
        const bool IS_MASTER = true;
        const string ISO_CODING_EN = "en";     

        private readonly IAsyncNotificationService _notificationService;
        
        private readonly LocalizationContext _localizationContext;
        private readonly UltraDBStrings _ultraDBStrings;
        private readonly UltraDBStrings2Context _ultraDBStrings2Context;
        private readonly UltraDBConcept _ultraDBConcept;
        private readonly XmlManager _xmlManager;
        
        public ConceptService(
            IAsyncNotificationService notificationService,
            LocalizationContext localizationContext,
            UltraDBStrings ultraDBStrings,
            UltraDBStrings2Context ultraDBStrings2Context,
            UltraDBConcept ultraDBConcept,
            XmlManager xmlManager)
        {
            _notificationService = notificationService;
            _localizationContext = localizationContext;
            _ultraDBStrings = ultraDBStrings;
            _ultraDBStrings2Context = ultraDBStrings2Context;
            _ultraDBConcept = ultraDBConcept;
            _xmlManager = xmlManager;
        }

        async public Task SaveAsync(SavableConceptModelDTO savableConceptModel)
        {
            if (savableConceptModel.Language.IsoCoding == ISO_CODING_EN)
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
            var xmlFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Constants.XML_FOLDER);
            _xmlManager.XmlDirectory = xmlFilePath;
            _xmlManager.LoadXmlOnly();
            _xmlManager.Completed.WaitOne();
            _xmlManager.ChangesFound = false;
            _xmlManager.InsertedCount = 0;
            _xmlManager.UpdatedCount = 0;
            _xmlManager.FillDB(_xmlManager.KeyTuplas);

            var result = new NewConceptsResult
            {
                InsertedCount = _xmlManager.InsertedCount,
                UpdatedCount = _xmlManager.UpdatedCount,
                IsNotified = true
            };
            if(_xmlManager.ChangesFound)
            {
                result.IsNotified = false;
                await _notificationService.ConceptsChanged(result);
            }
           
            return await Task.FromResult(result);
        }
    }
}
