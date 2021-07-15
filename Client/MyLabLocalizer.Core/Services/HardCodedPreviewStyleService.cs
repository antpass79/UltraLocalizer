using MyLabLocalizer.Core.Controls;
using MyLabLocalizer.Core.Models;
using MyLabLocalizer.Core.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MyLabLocalizer.Core.Services
{
    public class HardCodedPreviewStyleService : IPreviewStyleService
    {
        #region Constructors

        public HardCodedPreviewStyleService()
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
            get => this[contextName];
        }

        #endregion

        #region Private Functions

        private void InitializeMapping()
        {
            PreviewStyleMapping.Add("TS_AButton", new PreviewStyleInfo(DefaultPreviewStyleValues.ETouchScreenABtnFontSize1, DefaultPreviewStyleValues.ETouchScreenBtnFontWeight, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(140, 60)));
            PreviewStyleMapping.Add("TS_AButtonWithIcon", new PreviewStyleInfo(DefaultPreviewStyleValues.ETouchScreenABtnFontSize1, DefaultPreviewStyleValues.ETouchScreenBtnFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(140, 30)));
            PreviewStyleMapping.Add("TS_BButton", new PreviewStyleInfo(DefaultPreviewStyleValues.ETouchScreenBBtnFontSize1, DefaultPreviewStyleValues.ETouchScreenBtnFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(120, 20)));
            PreviewStyleMapping.Add("TS_TButton", new PreviewStyleInfo(DefaultPreviewStyleValues.ETouchScreenBtnFontSize1, DefaultPreviewStyleValues.ETouchScreenBtnFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(150, 30)));
            PreviewStyleMapping.Add("TS_SButton", new PreviewStyleInfo(DefaultPreviewStyleValues.ETouchScreenSBtnFontSize1, DefaultPreviewStyleValues.ETouchScreenBtnFontWeight, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(174, 76)));
            PreviewStyleMapping.Add("TS_TabName", new PreviewStyleInfo(DefaultPreviewStyleValues.ETouchScreenTabFontSize1, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(120, 35)));
            PreviewStyleMapping.Add("MD_RT11MeasName", new PreviewStyleInfo(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1, DefaultPreviewStyleValues.GDIEInfoWindowFontWeight1, DefaultPreviewStyleValues.GDIEditFontFamily1, false, new Size(70, 19)));
            PreviewStyleMapping.Add("MD_RT11UnitMeas", new PreviewStyleInfo(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1, DefaultPreviewStyleValues.GDIEInfoWindowFontWeight1, DefaultPreviewStyleValues.GDIEditFontFamily1, false, new Size(63, 19)));
            PreviewStyleMapping.Add("MD_RT11MeasHeader", new PreviewStyleInfo(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1, DefaultPreviewStyleValues.GDIEInfoWindowFontWeight1, DefaultPreviewStyleValues.GDIEditFontFamily1, false, new Size(186, 19)));
            PreviewStyleMapping.Add("MD_RT11MeasHeaderMagnified", new PreviewStyleInfo(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontSize1, DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1, DefaultPreviewStyleValues.GDIEditFontFamily1, false, new Size(160, 23)));
            PreviewStyleMapping.Add("MD_RT11MeasSpan2", new PreviewStyleInfo(DefaultPreviewStyleValues.GDIEInfoWindowVersion2FontSize1, DefaultPreviewStyleValues.GDIEInfoWindowFontWeight1, DefaultPreviewStyleValues.GDIEditFontFamily1, false, new Size(116, 19)));
            PreviewStyleMapping.Add("MD_RT14aTrackballStrings", new PreviewStyleInfo(DefaultPreviewStyleValues.EEditFontSize1, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(230, 20)));
            PreviewStyleMapping.Add("MD_RT13HelpLine", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowFontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(850, 20)));
            PreviewStyleMapping.Add("MD_RT8Info", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowFontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(50, 18)));
            PreviewStyleMapping.Add("MD_RT7Application", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowFontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(150, 26)));
            PreviewStyleMapping.Add("MD_RT910", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowFontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(70, 26)));
            PreviewStyleMapping.Add("MD_RT16TabColumnName", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowFontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(75, 50)));
            PreviewStyleMapping.Add("MD_RT17Label", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(63, 19)));
            PreviewStyleMapping.Add("MD_RT17LabelJoyPush", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowFontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(50, 18)));
            PreviewStyleMapping.Add("MD_RT17LabelJoyEnc", new PreviewStyleInfo(DefaultPreviewStyleValues.EInfoWindowVersion2FontSize1, DefaultPreviewStyleValues.EInfoWindowFontWeight1, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(58, 19)));
            PreviewStyleMapping.Add("Generic_Warning", new PreviewStyleInfo(DefaultPreviewStyleValues.EEditFontSize1, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(450, 200)));//multiline
            PreviewStyleMapping.Add("Generic_Long", new PreviewStyleInfo(DefaultPreviewStyleValues.EEditFontSize1, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(320, 30)));
            PreviewStyleMapping.Add("Generic_Medium", new PreviewStyleInfo(DefaultPreviewStyleValues.EEditFontSize1, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(160, 30)));
            PreviewStyleMapping.Add("Generic_Short", new PreviewStyleInfo(DefaultPreviewStyleValues.EEditFontSize1, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(80, 30)));
            PreviewStyleMapping.Add("MD_IconArea", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.ELabelStyle2FontWeight, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(140, 40)));
            PreviewStyleMapping.Add("MD_RT11ExtLabel", new PreviewStyleInfo(DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontSize1, DefaultPreviewStyleValues.GDIEInfoWindowMagnifiedFontWeight1, DefaultPreviewStyleValues.GDIEditFontFamily1, false, new Size(108, 23)));//96,23
            PreviewStyleMapping.Add("MC_GENERAL_ML_100PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(100, 40)));
            PreviewStyleMapping.Add("MC_GENERAL_ML_200PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(200, 40)));
            PreviewStyleMapping.Add("MC_GENERAL_ML_400PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, true, new Size(400, 40)));
            PreviewStyleMapping.Add("MC_GENERAL_30PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(30, 20)));
            PreviewStyleMapping.Add("MC_GENERAL_50PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(50, 20)));
            PreviewStyleMapping.Add("MC_GENERAL_100PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(100, 20)));
            PreviewStyleMapping.Add("MC_GENERAL_150PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(150, 20)));
            PreviewStyleMapping.Add("MC_GENERAL_200PX", new PreviewStyleInfo(DefaultPreviewStyleValues.ELabelStyle2FontSize, DefaultPreviewStyleValues.EStandardFontWeight, DefaultPreviewStyleValues.EditFontFamily1, false, new Size(200, 20)));
        }

        #endregion
    }
}
