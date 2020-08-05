﻿using Globe.Client.Platform.Controls;
using Globe.Client.Platform.Models;
using Globe.Client.Platform.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Globe.Client.Platform.Services
{
    public class RunTimePreviewStyleService : IPreviewStyleService
    {
        #region Data Members

        private string _customizableStylesDirectory;
        private string _defaultStylesDirectory;

        ResourceDictionary _commonResourceDictionary = new ResourceDictionary();

        Dictionary<string, int> _typeNameToMergedDictionaryIndexMapping = new Dictionary<string, int>();

        #endregion

        #region Constructors

        public RunTimePreviewStyleService()
        {
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
            InitPaths();
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
                EInfoWindowFontSize1 = customResourceDictionary.Contains( nameof(DefaultPreviewStyleValues.EInfoWindowFontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowFontSize1)] : EInfoWindowFontSize1;
                EInfoWindowVersion2FontSize1 = customResourceDictionary.Contains( nameof(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1)) ? customResourceDictionary[nameof(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1)] : EInfoWindowVersion2FontSize1;  
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

        #endregion

        #region Private Functions

        private void InitPaths()
        {
            _defaultStylesDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "DefaultStyles\\");
            _customizableStylesDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "CustomStyles\\");
        }

        private void InitResources()
        {
            LoadDefaultStyles(_defaultStylesDirectory, _commonResourceDictionary.MergedDictionaries);
            LoadCommonControlStyles(_defaultStylesDirectory, _commonResourceDictionary.MergedDictionaries);
            
            foreach (var resourceDictionary in _commonResourceDictionary.MergedDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            LoadCustomizableStyles(_customizableStylesDirectory, Application.Current.Resources.MergedDictionaries);
        }

        private void LoadDefaultStyles(string p_DefaultDictionaryDirectory, Collection<ResourceDictionary> p_TargetResources)
        {
            string l_DefaultStylesFilePath = p_DefaultDictionaryDirectory + @"\DefaultBasicStyles.xaml";
            ResourceDictionary l_ResourceDictionary;

            if (!System.IO.File.Exists(l_DefaultStylesFilePath))
            {
                throw new ApplicationException("Default Style File not found: " + l_DefaultStylesFilePath);
            }
            using (XmlReader l_XmlRead = XmlReader.Create(l_DefaultStylesFilePath))
            {
                l_ResourceDictionary = (ResourceDictionary)XamlReader.Load(l_XmlRead);
            }

            p_TargetResources.Add(l_ResourceDictionary);
        }

        private void LoadCommonControlStyles(string p_DefaultDictionaryDirectory, Collection<ResourceDictionary> p_TargetResources)
        {
           
            string l_uri2Path = @"/StyleManager;component/CommonControlsStyle.xaml";
            Uri l_uri = new Uri(l_uri2Path, UriKind.Relative);
            ResourceDictionary l_ResourceDictionary2 = (ResourceDictionary)Application.LoadComponent(l_uri);

            p_TargetResources.Add(l_ResourceDictionary2);
         
        }

        private void LoadCustomizableStyles(string p_CustomDictionaryDirectory, Collection<ResourceDictionary> p_TargetResources)
        {
            string[] filePaths = Directory.GetFiles(p_CustomDictionaryDirectory, "*Custom.xaml");
            
            //string[] filePaths = Directory.GetFiles(p_CustomDictionaryDirectory, "StandardV2_Custom.xaml");

            // Impossibile trovare la risorsa denominata 'TSButtonStyleCommon' (io la vedo definita sul CommonControlsStyle.xaml) 
            //string[] filePaths = Directory.GetFiles(p_CustomDictionaryDirectory, "Standard_Custom.xaml");


            foreach (string filePath in filePaths)
            {
                ResourceDictionary l_ResourceDictionaryCustom = new ResourceDictionary();

                using (XmlReader l_XmlRead = XmlReader.Create(filePath))
                {
                    l_ResourceDictionaryCustom = (ResourceDictionary)XamlReader.Load(l_XmlRead);
                }

                p_TargetResources.Add(l_ResourceDictionaryCustom);

                var typeName = Path.GetFileName(filePath).Replace("_Custom.xaml", string.Empty);
                _typeNameToMergedDictionaryIndexMapping.Add(typeName, p_TargetResources.Count - 1);

            }
        }

        #endregion
    }
}
