using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Porting.UltraDBDLL.XmlManager
{
    // ANTO fake, see NewXmlFormat for the auto generated
    // Additional Information, the structure is based on:
    // UIFramework/UsersManager/UsersManager/Localization/UIFramework.Users.Localization.definition.xml
    public class LocalizationResource
    {
        public string Language { get; set; }
        public decimal Version { get; set; }
        public string ComponentNamespace { get; set; }
        public IList<LocalizationSection> LocalizationSection { get; set; }

        public void Save(string file)
        {
            throw new NotImplementedException();
        }

        public static LocalizationResource Load(string file)
        {
            throw new NotImplementedException();
        }
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class LocalizationSection
    {
        public int Id { get; set; }
        public string InternalNamespace { get; set; }
        public IList<Concept> Concept { get; set; }
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class Concept
    {
        public string Id { get; set; }
        public IList<MyString> String { get; set; }
        public Comments Comments { get; set; }
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class MyString
    {
        public string Context { get; set; }
        public string TypedValue { get; set; }
        public int DatabaseID { get; set; }
        public bool IsAcceptable { get; set; }
    }

    // ANTO fake, see NewXmlFormat for the auto generated file
    public class Comments
    {
        public string TypedValue { get; set; }
    }
}