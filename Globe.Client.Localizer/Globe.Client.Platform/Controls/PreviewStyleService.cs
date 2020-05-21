using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Globe.Client.Platform.Controls
{
    public class PreviewStyleService
    {
        // FontSize
        const double ETouchScreenABtnFontSize1 = 20;
        const double ETouchScreenBBtnFontSize1 = 16;
        const double ETouchScreenBtnFontSize1 = 20;
        const double ETouchScreenSBtnFontSize1 = 18;
        const double ETouchScreenTabFontSize1 = 16;
        const double GDIEInfoWindowVersion2FontSize1 = 16;
        const double GDIEInfoWindowMagnifiedFontSize1 = 18;
        const double EEditFontSize1 = 14;
        const double EInfoWindowFontSize1 = 12;
        const double EInfoWindowVersion2FontSize1 = 15;
        const double ELabelStyle2FontSize = 13;

        // FontWeight
        readonly FontWeight ETouchScreenBtnFontWeight = FontWeights.Normal;
        readonly FontWeight EStandardFontWeight = FontWeights.Normal;
        readonly FontWeight GDIEInfoWindowFontWeight1 = FontWeights.Bold;
        readonly FontWeight GDIEInfoWindowMagnifiedFontWeight1 = FontWeights.Normal;
        readonly FontWeight EInfoWindowFontWeight1 = FontWeights.Bold;
        readonly FontWeight ELabelStyle2FontWeight = FontWeights.Bold;

        // FontFamily
        readonly FontFamily EditFontFamily1 = new FontFamily("Arial, SimHei, Verdana, Times new Roman");
        readonly FontFamily GDIEditFontFamily1 = new FontFamily("Arial, Verdana, Times new Roman");

        Dictionary<string, PreviewStyleInfo> _previewStyleMapping = new Dictionary<string, PreviewStyleInfo>();

        public PreviewStyleService()
        {
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            _previewStyleMapping.Add("TS_AButton", new PreviewStyleInfo(ETouchScreenABtnFontSize1, ETouchScreenBtnFontWeight, EditFontFamily1, true, new Size(140, 60)));
            _previewStyleMapping.Add("TS_AButtonWithIcon", new PreviewStyleInfo(ETouchScreenABtnFontSize1, ETouchScreenBtnFontWeight, EditFontFamily1, false, new Size(140, 30)));
            _previewStyleMapping.Add("TS_BButton", new PreviewStyleInfo(ETouchScreenBBtnFontSize1, ETouchScreenBtnFontWeight, EditFontFamily1, false, new Size(120, 20)));
            _previewStyleMapping.Add("TS_TButton", new PreviewStyleInfo(ETouchScreenBtnFontSize1, ETouchScreenBtnFontWeight, EditFontFamily1, false, new Size(150, 30)));
            _previewStyleMapping.Add("TS_SButton", new PreviewStyleInfo(ETouchScreenSBtnFontSize1, ETouchScreenBtnFontWeight, EditFontFamily1, true, new Size(174, 76)));
            _previewStyleMapping.Add("TS_TabName", new PreviewStyleInfo(ETouchScreenTabFontSize1, EStandardFontWeight, EditFontFamily1, false, new Size(120, 35)));
            _previewStyleMapping.Add("MD_RT11MeasName", new PreviewStyleInfo(GDIEInfoWindowVersion2FontSize1, GDIEInfoWindowFontWeight1, GDIEditFontFamily1, false, new Size(70, 19)));
            _previewStyleMapping.Add("MD_RT11UnitMeas", new PreviewStyleInfo(GDIEInfoWindowVersion2FontSize1, GDIEInfoWindowFontWeight1, GDIEditFontFamily1, false, new Size(63, 19)));
            _previewStyleMapping.Add("MD_RT11MeasHeader", new PreviewStyleInfo(GDIEInfoWindowVersion2FontSize1, GDIEInfoWindowFontWeight1, GDIEditFontFamily1, false, new Size(186, 19)));
            _previewStyleMapping.Add("MD_RT11MeasHeaderMagnified", new PreviewStyleInfo(GDIEInfoWindowMagnifiedFontSize1, GDIEInfoWindowMagnifiedFontWeight1, GDIEditFontFamily1, false, new Size(160, 23)));
            _previewStyleMapping.Add("MD_RT11MeasSpan2", new PreviewStyleInfo(GDIEInfoWindowVersion2FontSize1, GDIEInfoWindowFontWeight1, GDIEditFontFamily1, false, new Size(116, 19)));
            _previewStyleMapping.Add("MD_RT14aTrackballStrings", new PreviewStyleInfo(EEditFontSize1, EStandardFontWeight, EditFontFamily1, false, new Size(230, 20)));
            _previewStyleMapping.Add("MD_RT13HelpLine", new PreviewStyleInfo(EInfoWindowFontSize1, EInfoWindowFontWeight1, EditFontFamily1, false, new Size(850, 20)));
            _previewStyleMapping.Add("MD_RT8Info", new PreviewStyleInfo(EInfoWindowFontSize1, EInfoWindowFontWeight1, EditFontFamily1, false, new Size(50, 18)));
            _previewStyleMapping.Add("MD_RT7Application", new PreviewStyleInfo(EInfoWindowFontSize1, EInfoWindowFontWeight1, EditFontFamily1, false, new Size(150, 26)));
            _previewStyleMapping.Add("MD_RT910", new PreviewStyleInfo(EInfoWindowFontSize1, EInfoWindowFontWeight1, EditFontFamily1, false, new Size(70, 26)));
            _previewStyleMapping.Add("MD_RT16TabColumnName", new PreviewStyleInfo(EInfoWindowFontSize1, EInfoWindowFontWeight1, EditFontFamily1, true, new Size(75, 50)));
            _previewStyleMapping.Add("MD_RT17Label", new PreviewStyleInfo(EInfoWindowVersion2FontSize1, EInfoWindowFontWeight1, EditFontFamily1, false, new Size(63, 19)));
            _previewStyleMapping.Add("MD_RT17LabelJoyPush", new PreviewStyleInfo(EInfoWindowFontSize1, EInfoWindowFontWeight1, EditFontFamily1, false, new Size(50, 18)));
            _previewStyleMapping.Add("MD_RT17LabelJoyEnc", new PreviewStyleInfo(EInfoWindowVersion2FontSize1, EInfoWindowFontWeight1, EditFontFamily1, false, new Size(58, 19)));
            _previewStyleMapping.Add("Generic_Warning", new PreviewStyleInfo(EEditFontSize1, EStandardFontWeight, EditFontFamily1, true, new Size(450, 200)));//multiline
            _previewStyleMapping.Add("Generic_Long", new PreviewStyleInfo(EEditFontSize1, EStandardFontWeight, EditFontFamily1, false, new Size(320, 30)));
            _previewStyleMapping.Add("Generic_Medium", new PreviewStyleInfo(EEditFontSize1, EStandardFontWeight, EditFontFamily1, false, new Size(160, 30)));
            _previewStyleMapping.Add("Generic_Short", new PreviewStyleInfo(EEditFontSize1, EStandardFontWeight, EditFontFamily1, false, new Size(80, 30)));
            _previewStyleMapping.Add("MD_IconArea", new PreviewStyleInfo(ELabelStyle2FontSize, ELabelStyle2FontWeight, EditFontFamily1, true, new Size(140, 40)));
            _previewStyleMapping.Add("MD_RT11ExtLabel", new PreviewStyleInfo(GDIEInfoWindowMagnifiedFontSize1, GDIEInfoWindowMagnifiedFontWeight1, GDIEditFontFamily1, false, new Size(108, 23)));//96,23
            _previewStyleMapping.Add("MC_GENERAL_ML_100PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, true, new Size(100, 40)));
            _previewStyleMapping.Add("MC_GENERAL_ML_200PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, true, new Size(200, 40)));
            _previewStyleMapping.Add("MC_GENERAL_ML_400PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, true, new Size(400, 40)));
            _previewStyleMapping.Add("MC_GENERAL_30PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, false, new Size(30, 20)));
            _previewStyleMapping.Add("MC_GENERAL_50PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, false, new Size(50, 20)));
            _previewStyleMapping.Add("MC_GENERAL_100PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, false, new Size(100, 20)));
            _previewStyleMapping.Add("MC_GENERAL_150PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, false, new Size(150, 20)));
            _previewStyleMapping.Add("MC_GENERAL_200PX", new PreviewStyleInfo(ELabelStyle2FontSize, EStandardFontWeight, EditFontFamily1, false, new Size(200, 20)));
        }
    }
}
