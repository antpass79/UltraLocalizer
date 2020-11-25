using Globe.Client.Platform.Controls;
using Globe.Client.Platform.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Globe.Client.Platform.Services
{
    public class RunTimePreviewStyleService : IPreviewStyleService
    {
        #region Data Members

        string PATH_DEFAULT_BASIC_STYLE = string.Empty;
        string ROOT_PATH_CUSTOM_STYLE = string.Empty;
        string PATH_COMMON_CONTROL_STYLE = @"/StyleManager;component/CommonControlsStyle.xaml";

        string[] _customFileNames = new string[]
            {
                "Veterinary_Custom.xaml",
                "Standard_Custom.xaml",
                "StandardV2_Custom.xaml",
                "OrangeGrey_Custom.xaml"
            };

        ResourceDictionary _commonResourceDictionary = new ResourceDictionary();

        Dictionary<string, int> _typeNameToMergedDictionaryIndexMapping = new Dictionary<string, int>();

        #endregion

        #region Constructors

        public RunTimePreviewStyleService(string baseDirectory = "")
        {
            baseDirectory = string.IsNullOrWhiteSpace(baseDirectory) ? AppDomain.CurrentDomain.BaseDirectory : baseDirectory;
            PATH_DEFAULT_BASIC_STYLE = $"{baseDirectory}/Styles/DefaultStyles/DefaultBasicStyles.xaml";
            ROOT_PATH_CUSTOM_STYLE = $"{baseDirectory}/Styles/CustomStyles";

            InitializeMapping();
        }

        #endregion

        #region Properties

        protected Dictionary<string, PreviewStyleInfo> PreviewStyleMapping { get; } = new Dictionary<string, PreviewStyleInfo>();

        public PreviewStyleInfo this[string contextName]
        {
            get => PreviewStyleMapping[contextName];
        }

        public PreviewStyleInfo this[string typeName, string contextName]
        {
            get => PreviewStyleMapping[$"{typeName}{contextName}"];
        }

        #endregion

        #region Private Functions

        private void InitializeMapping()
        {
            InitResources();

            //Font Size
            var ETouchScreenABtnFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenABtnFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenABtnFontSize1)] : DefaultPreviewStyleValues.ETouchScreenABtnFontSize1;
            var ETouchScreenBBtnFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenBBtnFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenBBtnFontSize1)] : DefaultPreviewStyleValues.ETouchScreenBBtnFontSize1;
            var ETouchScreenBtnFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontSize1)] : DefaultPreviewStyleValues.ETouchScreenBtnFontSize1;
            var ETouchScreenSBtnFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenSBtnFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenSBtnFontSize1)] : DefaultPreviewStyleValues.ETouchScreenSBtnFontSize1;
            var ETouchScreenTabFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenTabFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenTabFontSize1)] : DefaultPreviewStyleValues.ETouchScreenTabFontSize1;
            var GDIEInfoWindowVersion2FontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1)] : DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1;
            var GDIEInfoWindowMagnifiedFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontSize1)] : DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontSize1;
            var EEditFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EEditFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.EEditFontSize1)] : DefaultPreviewStyleValues.EEditFontSize1;
            var EInfoWindowFontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowFontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowFontSize1)] : DefaultPreviewStyleValues.EInfoWindowFontSize1;
            var EInfoWindowVersion2FontSize1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1)] : DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1;
            var ELabelStyle2FontSize = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ELabelStyle2FontSize)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.ELabelStyle2FontSize)] : DefaultPreviewStyleValues.ELabelStyle2FontSize;

            //Font Weight
            var ETouchScreenBtnFontWeight = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontWeight)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontWeight)] : DefaultPreviewStyleValues.ETouchScreenBtnFontWeight;
            var EStandardFontWeight = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EStandardFontWeight)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.EStandardFontWeight)] : DefaultPreviewStyleValues.EStandardFontWeight;
            var GDIEInfoWindowFontWeight1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowFontWeight1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1)] : DefaultPreviewStyleValues.GDIEInfoWindowFontWeight1;
            var GDIEInfoWindowMagnifiedFontWeight1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1)] : DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1;
            var EInfoWindowFontWeight1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)] : DefaultPreviewStyleValues.EInfoWindowFontWeight1;
            var ELabelStyle2FontWeight = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)] : DefaultPreviewStyleValues.EInfoWindowFontWeight1;

            //Font Family
            var EditFontFamily1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EditFontFamily1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.EditFontFamily1)] : DefaultPreviewStyleValues.EditFontFamily1;
            var GDIEditFontFamily1 = _commonResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEditFontFamily1)) ? _commonResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEditFontFamily1)] : DefaultPreviewStyleValues.GDIEditFontFamily1;

            foreach (KeyValuePair<string, int> pair in _typeNameToMergedDictionaryIndexMapping)
            {
                var customResourceDictionary = Application.Current.Resources.MergedDictionaries[pair.Value];

                //Font Size
                ETouchScreenABtnFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenABtnFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenABtnFontSize1)] : ETouchScreenABtnFontSize1;
                ETouchScreenBBtnFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenBBtnFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenBBtnFontSize1)] : ETouchScreenBBtnFontSize1;
                ETouchScreenBtnFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontSize1)] : ETouchScreenBtnFontSize1;
                ETouchScreenSBtnFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenSBtnFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenSBtnFontSize1)] : ETouchScreenSBtnFontSize1;
                ETouchScreenTabFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenTabFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenTabFontSize1)] : ETouchScreenTabFontSize1;
                GDIEInfoWindowVersion2FontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1)] : GDIEInfoWindowVersion2FontSize1;
                GDIEInfoWindowMagnifiedFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontSize1)] : GDIEInfoWindowMagnifiedFontSize1;
                EEditFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EEditFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EEditFontSize1)] : EEditFontSize1;
                EInfoWindowFontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowFontSize1)] : EInfoWindowFontSize1;
                EInfoWindowVersion2FontSize1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1)] : EInfoWindowVersion2FontSize1;
                ELabelStyle2FontSize = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ELabelStyle2FontSize)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.ELabelStyle2FontSize)] : ELabelStyle2FontSize;

                //Font Weight
                ETouchScreenBtnFontWeight = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontWeight)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.ETouchScreenBtnFontWeight)] : ETouchScreenBtnFontWeight;
                EStandardFontWeight = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EStandardFontWeight)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EStandardFontWeight)] : EStandardFontWeight;
                GDIEInfoWindowFontWeight1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowFontWeight1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1)] : GDIEInfoWindowFontWeight1;
                GDIEInfoWindowMagnifiedFontWeight1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1)] : GDIEInfoWindowMagnifiedFontWeight1;
                EInfoWindowFontWeight1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)] : EInfoWindowFontWeight1;
                ELabelStyle2FontWeight = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowFontWeight1)] : EInfoWindowFontWeight1;

                //Font Family
                EditFontFamily1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.EditFontFamily1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EditFontFamily1)] : EditFontFamily1;
                GDIEditFontFamily1 = customResourceDictionary.Contains(nameof(DefaultPreviewStyleValues.GDIEditFontFamily1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.GDIEditFontFamily1)] : GDIEditFontFamily1;

                PreviewStyleMapping.Add($"{pair.Key}TS_AButton", new PreviewStyleInfo((double)ETouchScreenABtnFontSize1, (FontWeight)ETouchScreenBtnFontWeight, (FontFamily)EditFontFamily1, true, new Size(140, 60)));
                PreviewStyleMapping.Add($"{pair.Key}TS_AButtonWithIcon", new PreviewStyleInfo((double)ETouchScreenABtnFontSize1, (FontWeight)ETouchScreenBtnFontWeight, (FontFamily)EditFontFamily1, false, new Size(140, 30)));
                PreviewStyleMapping.Add($"{pair.Key}TS_BButton", new PreviewStyleInfo((double)ETouchScreenBBtnFontSize1, (FontWeight)ETouchScreenBtnFontWeight, (FontFamily)EditFontFamily1, false, new Size(120, 20)));
                PreviewStyleMapping.Add($"{pair.Key}TS_TButton", new PreviewStyleInfo((double)ETouchScreenBtnFontSize1, (FontWeight)ETouchScreenBtnFontWeight, (FontFamily)EditFontFamily1, false, new Size(150, 30)));
                PreviewStyleMapping.Add($"{pair.Key}TS_SButton", new PreviewStyleInfo((double)ETouchScreenSBtnFontSize1, (FontWeight)ETouchScreenBtnFontWeight, (FontFamily)EditFontFamily1, true, new Size(174, 76)));
                PreviewStyleMapping.Add($"{pair.Key}TS_TabName", new PreviewStyleInfo((double)ETouchScreenTabFontSize1, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(120, 35)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT11MeasName", new PreviewStyleInfo((double)GDIEInfoWindowVersion2FontSize1, (FontWeight)GDIEInfoWindowFontWeight1, (FontFamily)GDIEditFontFamily1, false, new Size(70, 19)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT11UnitMeas", new PreviewStyleInfo((double)GDIEInfoWindowVersion2FontSize1, (FontWeight)GDIEInfoWindowFontWeight1, (FontFamily)GDIEditFontFamily1, false, new Size(63, 19)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT11MeasHeader", new PreviewStyleInfo((double)GDIEInfoWindowVersion2FontSize1, (FontWeight)GDIEInfoWindowFontWeight1, (FontFamily)GDIEditFontFamily1, false, new Size(186, 19)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT11MeasHeaderMagnified", new PreviewStyleInfo((double)GDIEInfoWindowMagnifiedFontSize1, (FontWeight)GDIEInfoWindowMagnifiedFontWeight1, (FontFamily)GDIEditFontFamily1, false, new Size(160, 23)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT11MeasSpan2", new PreviewStyleInfo((double)GDIEInfoWindowVersion2FontSize1, (FontWeight)GDIEInfoWindowFontWeight1, (FontFamily)GDIEditFontFamily1, false, new Size(116, 19)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT14aTrackballStrings", new PreviewStyleInfo((double)EEditFontSize1, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(230, 20)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT13HelpLine", new PreviewStyleInfo((double)EInfoWindowFontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, false, new Size(850, 20)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT8Info", new PreviewStyleInfo((double)EInfoWindowFontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, false, new Size(50, 18)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT7Application", new PreviewStyleInfo((double)EInfoWindowFontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, false, new Size(150, 26)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT910", new PreviewStyleInfo((double)EInfoWindowFontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, false, new Size(70, 26)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT16TabColumnName", new PreviewStyleInfo((double)EInfoWindowFontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, true, new Size(75, 50)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT17Label", new PreviewStyleInfo((double)EInfoWindowVersion2FontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, false, new Size(63, 19)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT17LabelJoyPush", new PreviewStyleInfo((double)EInfoWindowFontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, false, new Size(50, 18)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT17LabelJoyEnc", new PreviewStyleInfo((double)EInfoWindowVersion2FontSize1, (FontWeight)EInfoWindowFontWeight1, (FontFamily)EditFontFamily1, false, new Size(58, 19)));
                PreviewStyleMapping.Add($"{pair.Key}Generic_Warning", new PreviewStyleInfo((double)EEditFontSize1, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, true, new Size(450, 200)));//multiline
                PreviewStyleMapping.Add($"{pair.Key}Generic_Long", new PreviewStyleInfo((double)EEditFontSize1, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(320, 30)));
                PreviewStyleMapping.Add($"{pair.Key}Generic_Medium", new PreviewStyleInfo((double)EEditFontSize1, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(160, 30)));
                PreviewStyleMapping.Add($"{pair.Key}Generic_Short", new PreviewStyleInfo((double)EEditFontSize1, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(80, 30)));
                PreviewStyleMapping.Add($"{pair.Key}MD_IconArea", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)ELabelStyle2FontWeight, (FontFamily)EditFontFamily1, true, new Size(140, 40)));
                PreviewStyleMapping.Add($"{pair.Key}MD_RT11ExtLabel", new PreviewStyleInfo((double)GDIEInfoWindowMagnifiedFontSize1, (FontWeight)GDIEInfoWindowMagnifiedFontWeight1, (FontFamily)GDIEditFontFamily1, false, new Size(108, 23)));//96,23
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_ML_100PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, true, new Size(100, 40)));
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_ML_200PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, true, new Size(200, 40)));
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_ML_400PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, true, new Size(400, 40)));
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_30PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(30, 20)));
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_50PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(50, 20)));
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_100PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(100, 20)));
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_150PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(150, 20)));
                PreviewStyleMapping.Add($"{pair.Key}MC_GENERAL_200PX", new PreviewStyleInfo((double)ELabelStyle2FontSize, (FontWeight)EStandardFontWeight, (FontFamily)EditFontFamily1, false, new Size(200, 20)));
            }
        }

        private void InitResources()
        {
            LoadStyles(_commonResourceDictionary.MergedDictionaries, PATH_DEFAULT_BASIC_STYLE);
            LoadStyles(_commonResourceDictionary.MergedDictionaries, PATH_COMMON_CONTROL_STYLE, true);
  
            foreach (var resourceDictionary in _commonResourceDictionary.MergedDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            LoadCustomizableStyles(Application.Current.Resources.MergedDictionaries);
        }

        private ResourceDictionary LoadSkin(string FilePath)
        {   
            using var XmlRead = XmlReader.Create(FilePath);
            return XamlReader.Load(XmlRead) as ResourceDictionary;
        }

        private void LoadStyles(Collection<ResourceDictionary> targetResources, string path, bool embedded = false)
        {
            ResourceDictionary resourceDictionary;
            if (embedded)
            {
                Uri uri = new Uri(path, UriKind.Relative);
                resourceDictionary = (ResourceDictionary)Application.LoadComponent(uri);
            }
            else
                resourceDictionary = LoadSkin(path);

            targetResources.Add(resourceDictionary);
        }

        private void LoadCustomizableStyles(Collection<ResourceDictionary> targetResources)
        {
            foreach (string customFileName in _customFileNames)
            {
                LoadStyles(targetResources, $"{ROOT_PATH_CUSTOM_STYLE}/{customFileName}");

                var typeName = customFileName.Replace("_Custom.xaml", string.Empty);
                _typeNameToMergedDictionaryIndexMapping.Add(typeName, targetResources.Count - 1);
            }
        }

        #endregion
    }
}
