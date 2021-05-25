using Globe.Client.Platform.Models;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class RemotePreviewStyleService : IPreviewStyleService
    {
        #region Data Members

        private readonly IStyleService _styleService;
        private readonly IPreviewStyleService _runTimePreviewStyleService;
        readonly string PATH_DEFAULT_BASIC_STYLE = $"{AppDomain.CurrentDomain.BaseDirectory}/Styles/";
        readonly string[] _stylePaths = new string[]
            {
                "CustomStyles/Veterinary_Custom.xaml",
                "CustomStyles/Standard_Custom.xaml",
                "CustomStyles/StandardV2_Custom.xaml",
                "CustomStyles/OrangeGrey_Custom.xaml",
                "DefaultStyles/DefaultBasicStyles.xaml"
            };

        #endregion

        #region Constructors

        public RemotePreviewStyleService(IStyleService styleService)
        {
            _styleService = styleService;

            bool exist = Directory.Exists(PATH_DEFAULT_BASIC_STYLE);
            if (!exist)
            {
                Directory.CreateDirectory(Path.Combine(PATH_DEFAULT_BASIC_STYLE, "CustomStyles"));
                Directory.CreateDirectory(Path.Combine(PATH_DEFAULT_BASIC_STYLE, "DefaultStyles"));
            }

            if (!exist)
            {
                foreach (var stylePath in _stylePaths)
                {
                    var path = $"{PATH_DEFAULT_BASIC_STYLE}{stylePath}";
                    if (File.Exists(path))
                        File.Delete(path);

                    Task.Run(async () =>
                    {
                        var content = await _styleService.Get(stylePath);
                        File.WriteAllText(path, content);
                    }).Wait();
                }
            };

            _runTimePreviewStyleService = new RunTimePreviewStyleService(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
        }

        #endregion

        #region Properties

        public PreviewStyleInfo this[string contextName]
        {
            get => _runTimePreviewStyleService?[contextName];
        }

        public PreviewStyleInfo this[string typeName, string contextName]
        {
            get => _runTimePreviewStyleService?[$"{typeName}{contextName}"];
        }

        #endregion
    }
}
