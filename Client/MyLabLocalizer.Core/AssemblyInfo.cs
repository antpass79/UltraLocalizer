using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.mylablocalizer.com/", "MyLabLocalizer.Core")]
[assembly: XmlnsDefinition("http://schemas.mylablocalizer.com/", "MyLabLocalizer.Core.Controls")]
[assembly: XmlnsDefinition("http://schemas.mylablocalizer.com/", "MyLabLocalizer.Core.Views")]
[assembly: XmlnsDefinition("http://schemas.mylablocalizer.com/", "MyLabLocalizer.Core.Converters")]
[assembly: XmlnsDefinition("http://schemas.mylablocalizer.com/", "MyLabLocalizer.Core.Behaviours")]
[assembly: XmlnsDefinition("http://schemas.mylablocalizer.com/", "MyLabLocalizer.Core.Utilities")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]
