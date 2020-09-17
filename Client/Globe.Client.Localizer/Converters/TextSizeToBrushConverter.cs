using Globe.Client.Platform.Controls;
using Globe.Client.Platform.Models;
using Globe.Client.Platform.Services;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Globe.Client.Localizer.Converters
{
    class TextSizeToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var contextName = values[0] as string;
            var previewStyleService = values[1] as IPreviewStyleService;
            var text = values[2] as string;
            var textBox = (values[3] as PreviewString).InnerTextBox;
            var typeName = parameter as string;

            var previewStyleInfo = previewStyleService[typeName, contextName];

            text = string.IsNullOrEmpty(text) ? string.Empty : text;

            bool lightGreen = CheckStringDimension(text, textBox, previewStyleInfo, textBox.Padding.Left, textBox.Padding.Right, textBox.Padding.Top, textBox.Padding.Bottom, culture);

            if (lightGreen)
                return Brushes.Green;
            else
                return Brushes.Red;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Transforms pixels to device independent units (1/96 of an inch)
        /// </summary>
        private void TransformPixelsToDIPs(double pixelX,
                                          double pixelY,
                                          out double unitX,
                                          out double unitY)
        {

            Matrix m = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
            double dpiX = m.M11; //Questo e' gia il rapporto 120/96 = 1.25
            double dpiY = m.M22;

            //DIPs = pixels / (DPI/96.0)
            //unitX = pixelX / (dpiX / 96);
            //unitY = pixelY / (dpiY / 96);

            unitX = pixelX / dpiX;
            unitY = pixelY / dpiY;
        }

        private double StringWidth(string text, TextBox textBox, PreviewStyleInfo previewStyleInfo, CultureInfo cultureInfo)
        {
            //FontFamily = TypeFace? TODO
            Typeface typeface = new Typeface(previewStyleInfo.FontFamily.ToString());

            FormattedText formattedText = new FormattedText(text, cultureInfo,
            FlowDirection.LeftToRight, typeface, previewStyleInfo.FontSize, Brushes.Black,
            VisualTreeHelper.GetDpi(textBox).PixelsPerDip);

            return formattedText.Width;
        }

        private double StringHeight(string text, TextBox textBox, PreviewStyleInfo previewStyleInfo, CultureInfo cultureInfo)
        {
            Typeface typeface = new Typeface(previewStyleInfo.FontFamily.ToString());

            FormattedText formattedText = new FormattedText(text, cultureInfo,
            FlowDirection.LeftToRight, typeface, previewStyleInfo.FontSize, Brushes.Black,
            VisualTreeHelper.GetDpi(textBox).PixelsPerDip);

            return formattedText.Height;
        }

        private bool CheckStringDimension(string text, TextBox textBox, PreviewStyleInfo previewStyleInfo, double pixelPaddingLeft, double pixelPaddingRight, double pixelPaddingTop, double pixelPaddingBottom, CultureInfo cultureInfo)
        {
            double stringWidth = StringWidth(text, textBox, previewStyleInfo, cultureInfo);
            double stringHeight = StringHeight(text, textBox, previewStyleInfo, cultureInfo);

            //-------------- Inizio Test---------------------
  
            //Size wannaBe = this.PreviewStyleInfo.Size;

            //double DIPsXPROVA = 0.0d;
            //double DIPsYPROVA = 0.0d;

            //TransformPixelsToDIPs(wannaBe.Width, wannaBe.Height, out DIPsXPROVA, out DIPsYPROVA);

            //bool p = (DIPsXPROVA != Width);

            //--------------Fine Test---------------------

            //Convert pixelMarginLeft -> device-independent units (DIPs)
            double paddingDIPsXleft = 0.0d;
            double paddingDIPsYtop = 0.0d;
            double paddingDIPsXright = 0.0d;
            double paddingDIPsYbottom = 0.0d;

            TransformPixelsToDIPs(pixelPaddingLeft, pixelPaddingTop, out paddingDIPsXleft, out paddingDIPsYtop);
            TransformPixelsToDIPs(pixelPaddingRight, pixelPaddingBottom, out paddingDIPsXright, out paddingDIPsYbottom);

            if (textBox.ActualWidth > stringWidth + paddingDIPsXleft + paddingDIPsXright)
            {
                if (textBox.ActualHeight > stringHeight + paddingDIPsYtop + paddingDIPsYbottom)
                {
                    //Tecnicamente starei in una riga sola, percio' se sono o non son multiline, non mi interessa
                    return true;
                }
                else
                    return false;
            }
            else if (previewStyleInfo.Multiline)
            {
                //Numero di righe che possono essere contenute dall'elemento grafico
                int numRows = (int)(textBox.ActualWidth / (stringHeight + paddingDIPsYtop + paddingDIPsYbottom));
                if (textBox.ActualWidth > ((stringWidth + paddingDIPsXleft + paddingDIPsXright) / numRows))
                    return true;
                else
                    return false;
            }
            else
                return false;

        }

    }
}
