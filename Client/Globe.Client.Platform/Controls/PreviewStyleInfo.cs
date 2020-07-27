using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Globe.Client.Platform.Controls
{
    public class PreviewStyleInfo
    {
        public double FontSize { get; }
        public FontWeight FontWeight { get; }
        public FontFamily FontFamily { get; }
        public bool Multiline { get; }
        public Size Size { get; }

        public PreviewStyleInfo(double fontSize, FontWeight fontWeight, FontFamily fontFamily, bool multiline, Size size)
        {
            FontSize = fontSize;
            FontWeight = fontWeight;
            FontFamily = fontFamily;
            Multiline = multiline;
            Size = size;
        }

        static internal PreviewStyleInfo Default()
        {
            return new PreviewStyleInfo(12, FontWeights.Normal, new FontFamily("Arial, SimHei, Verdana, Times new Roman"), false, new Size(140, 60));
        }
    }
}
