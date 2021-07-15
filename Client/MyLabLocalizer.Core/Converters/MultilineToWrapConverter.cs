using System.Windows;

namespace MyLabLocalizer.Core.Converters
{
    public class MultilineToWrapConverter : BooleanConverter<TextWrapping>
    {
        public MultilineToWrapConverter()
            : base (TextWrapping.Wrap, TextWrapping.NoWrap)
        {
        }
    }
}
