using Globe.Client.Platform.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class RemotePreviewStyleService : IPreviewStyleService
    {
        #region Data Members

        private readonly IStyleService _styleService;
        private readonly ICompareVersionService _compareVersionService;
        private readonly IPreviewStyleService _runTimePreviewStyleService;

        string PATH_DEFAULT_BASIC_STYLE = $"{AppDomain.CurrentDomain.BaseDirectory}/Styles/";

        string[] _stylePaths = new string[]
            {
                "CustomStyles/Veterinary_Custom.xaml",
                "CustomStyles/Standard_Custom.xaml",
                "CustomStyles/StandardV2_Custom.xaml",
                "CustomStyles/OrangeGrey_Custom.xaml",
                "DefaultStyles/DefaultBasicStyles.xaml"
            };

        #endregion

        #region Constructors

        public RemotePreviewStyleService(IStyleService styleService, ICompareVersionService compareVersionService)
        {
            _styleService = styleService;
            _compareVersionService = compareVersionService;

            Task.Run(async () =>
            {
                if (await _compareVersionService.NewXamlVersionAvailable())
                {
                    foreach (var stylePath in _stylePaths)
                        {
                            var path = $"{PATH_DEFAULT_BASIC_STYLE}{stylePath}";
                            if (File.Exists(path))
                                File.Delete(path);

                            var content = await _styleService.Get(stylePath);

                            File.WriteAllText(path, content);
                        }
                }
            }).Wait();

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
