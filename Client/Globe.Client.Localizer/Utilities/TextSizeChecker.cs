using Globe.Client.Platform.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Globe.Client.Localizer.Utilities
{
    class TextSizeChecker
    {
        #region Public Functions

        public bool Check(string text, TextBlock textBlock, PreviewStyleInfo previewStyleInfo, double pixelPaddingLeft, double pixelPaddingRight, double pixelPaddingTop, double pixelPaddingBottom, CultureInfo cultureInfo)
        {
            double textWidth = TextSize(text, textBlock, previewStyleInfo, cultureInfo).Width;
            double textHeight = TextSize(text, textBlock, previewStyleInfo, cultureInfo).Height;

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

            if (textBlock.ActualWidth > textWidth + paddingDIPsXleft + paddingDIPsXright)
            {
                if (textBlock.ActualHeight > textHeight + paddingDIPsYtop + paddingDIPsYbottom)
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
                int numRows = (int)(textBlock.ActualWidth / (textHeight + paddingDIPsYtop + paddingDIPsYbottom));
                if (textBlock.ActualWidth > ((textWidth + paddingDIPsXleft + paddingDIPsXright) / numRows))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region Private Functions

        private static Size TextSize(string text, TextBlock textBlock, PreviewStyleInfo previewStyleInfo, CultureInfo cultureInfo)
        {
            Typeface typeface = new Typeface(previewStyleInfo.FontFamily.ToString());

            FormattedText formattedText = new FormattedText(
                text,
                cultureInfo,
                FlowDirection.LeftToRight,
                typeface,
                previewStyleInfo.FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);

            return new Size(formattedText.Width, formattedText.Height);
        }

        /// <summary>
        /// Transforms pixels to device independent units (1/96 of an inch)
        /// </summary>
        private static void TransformPixelsToDIPs(double pixelX,
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

        #endregion
    }
}
