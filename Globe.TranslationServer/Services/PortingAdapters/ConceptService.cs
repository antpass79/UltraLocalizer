using DocumentFormat.OpenXml.InkML;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.Adapters;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using System;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.PortingAdapters
{
    public class ConceptService : IAsyncConceptService
    {
        const string ISO_CODING_EN = "en";
        const bool IS_MASTER = true;

        private readonly LocalizationContext _localizationContext;
        private readonly UltraDBStrings _ultraDBStrings;
        private readonly UltraDBStrings2Context _ultraDBStrings2Context;
        private readonly UltraDBConcept _ultraDBConcept;

        public ConceptService(
            LocalizationContext localizationContext,
            UltraDBStrings ultraDBStrings,
            UltraDBStrings2Context ultraDBStrings2Context,
            UltraDBConcept ultraDBConcept)
        {
            _localizationContext = localizationContext;
            _ultraDBStrings = ultraDBStrings;
            _ultraDBStrings2Context = ultraDBStrings2Context;
            _ultraDBConcept = ultraDBConcept;
        }

        async public Task SaveAsync(SavableConceptModelDTO savableConceptModel)
        {
            if (savableConceptModel.Language.ISOCoding == ISO_CODING_EN)
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
                        var stringId = _ultraDBStrings.InsertNewString(1, (int)context.StringType, context.StringValue);
                        _ultraDBStrings2Context.InsertNewStrings2Context(stringId, context.Concept2ContextId);
                        Console.WriteLine($"New string {context.StringValue} has been inserted with type ID = {context.StringType}");
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
                UltraDBExtendedStrings.Languages language = UltraDBExtendedStrings.ParseFromString(savableConceptModel.Language.ISOCoding);
                foreach (var context in savableConceptModel.Concept.EditableContexts)
                {
                    // Nothing happens
                    if (context.OldStringId != 0 && context.StringDefaultValue == context.StringValue)
                    {
                        continue;
                    }

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

            await Task.CompletedTask;
        }
    }
}
