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

        public static bool Check(string text, TextBlock textBlock, PreviewStyleInfo previewStyleInfo, double pixelPaddingLeft, double pixelPaddingRight, double pixelPaddingTop, double pixelPaddingBottom, CultureInfo cultureInfo)
        {
            var size = TextSize(text, textBlock, previewStyleInfo, cultureInfo);

            var (dpiPaddingLeft, dpiPaddingTop) = TransformPixelsToDIPs(pixelPaddingLeft, pixelPaddingTop);
            var (dpiPaddingRight, dpiPaddingBottom) = TransformPixelsToDIPs(pixelPaddingRight, pixelPaddingBottom);

            if (previewStyleInfo.Size.Width > size.Width + dpiPaddingLeft + dpiPaddingRight &&
                previewStyleInfo.Size.Height > size.Height + dpiPaddingTop + dpiPaddingBottom)
                return true;

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
                textBlock.FlowDirection,
                typeface,
                previewStyleInfo.FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(textBlock).PixelsPerDip)
            {
                MaxTextWidth = previewStyleInfo.Size.Width,
                TextAlignment = textBlock.TextAlignment,
                Trimming = textBlock.TextTrimming                
            };

            return new Size(formattedText.Width, formattedText.Height);
        }

        /// <summary>
        /// Transforms pixels to device independent units (1/96 of an inch)
        /// </summary>
        private static (double, double) TransformPixelsToDIPs(
            double pixelX,
            double pixelY)
        {
            Matrix m = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
            double dpiX = m.M11; // it's the ratio 120/96 = 1.25
            double dpiY = m.M22;

            //DIPs = pixels / (DPI/96.0)
            //unitX = pixelX / (dpiX / 96);
            //unitY = pixelY / (dpiY / 96);

            return (pixelX / dpiX, pixelY / dpiY);
        }

        #endregion
    }
}
