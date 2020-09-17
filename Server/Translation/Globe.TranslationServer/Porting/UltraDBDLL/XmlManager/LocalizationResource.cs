﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Globe.TranslationServer.Porting.UltraDBDLL.XmlManager
{
    // ANTO fake, see NewXmlFormat for the auto generated
    // Additional Information, the structure is based on:
    // UIFramework/UsersManager/UsersManager/Localization/UIFramework.Users.Localization.definition.xml
    public class LocalizationResource
    {
        const string ATTRIBUTE_COMPONENT_NAMESPACE = "ComponentNamespace";
        const string ATTRIBUTE_LANGUAGE = "Language";
        const string ATTRIBUTE_VERSION = "Version";

        const string ATTRIBUTE_INTERNAL_NAMESPACE = "InternalNamespace";
        const string ATTRIBUTE_CONCEPT_ID = "Id";

        const string ATTRIBUTE_CONTEXT = "Context";

        const string TAG_LOCALIZATION_SECTION = "LocalizationSection";
        const string TAG_CONCEPT = "Concept";
        const string TAG_COMMENTS = "Comments";
        const string TAG_STRING = "String";

        public string Language { get; set; }
        public decimal Version { get; set; }
        public string ComponentNamespace { get; set; }
        public IList<LocalizationSection> LocalizationSection { get; set; } = new List<LocalizationSection>();

        public void Save(string file)
        {
            throw new NotImplementedException();
        }

        public static LocalizationResource Load(string file)
        {
            LocalizationResource localizationResource = new LocalizationResource();

            XDocument document = XDocument.Load(File.OpenText(file), LoadOptions.PreserveWhitespace);
            var componentNamespace = document.Root.Attribute(ATTRIBUTE_COMPONENT_NAMESPACE);
            var language = document.Root.Attribute(ATTRIBUTE_LANGUAGE);
            var version = document.Root.Attribute(ATTRIBUTE_VERSION);

            localizationResource.ComponentNamespace = componentNamespace.Value;
            localizationResource.Language = language.Value;
            localizationResource.Version = decimal.Parse(version.Value, System.Globalization.CultureInfo.InvariantCulture);

            var localizationSectionTags = document.Descendants(TAG_LOCALIZATION_SECTION);

            foreach (var localizationSectionTag in localizationSectionTags)
            {
                LocalizationSection localizationSection = new LocalizationSection();
                localizationResource.LocalizationSection.Add(localizationSection);

                var internalNamespace = localizationSectionTag.Attribute(ATTRIBUTE_INTERNAL_NAMESPACE);
                localizationSection.InternalNamespace = internalNamespace != null ? internalNamespace.Value : string.Empty;

                var conceptTags = localizationSectionTag.Descendants(TAG_CONCEPT);

                foreach (var conceptTag in conceptTags)
                {
                    Concept concept = new Concept();
                    localizationSection.Concept.Add(concept);
                    var conceptId = conceptTag.Attribute(ATTRIBUTE_CONCEPT_ID);
                    concept.Id = conceptId.Value;

                    var commentsTag = conceptTag.Element(TAG_COMMENTS);
                    concept.Comments = new Comments
                    {
                        TypedValue = commentsTag != null ? commentsTag.Value : string.Empty
                    };

                    var stringTags = conceptTag.Descendants(TAG_STRING);
                    foreach (var stringTag in stringTags)
                    {
                        TagString tagString = new TagString();
                        concept.String.Add(tagString);

                        var context = stringTag.Attribute(ATTRIBUTE_CONTEXT);
                        tagString.Context = context.Value;
                        tagString.TypedValue = stringTag.Value;
                    }
                }
            }

            return localizationResource;
        }
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class LocalizationSection
    {
        public string InternalNamespace { get; set; }
        public IList<Concept> Concept { get; set; } = new List<Concept>();
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class Concept
    {
        public string Id { get; set; }
        public IList<TagString> String { get; set; } = new List<TagString>();
        public Comments Comments { get; set; }
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class TagString
    {
        public string Context { get; set; }
        public string TypedValue { get; set; }
        public int? DatabaseID { get; set; }
        public bool? IsAcceptable { get; set; }
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class Comments
    {
        public string TypedValue { get; set; }
    }
}