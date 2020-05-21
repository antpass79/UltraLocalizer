using System.Windows;

namespace Globe.Client.Platform.Converters
{
    public class MultilineToWrapConverter : BooleanConverter<TextWrapping>
    {
        public MultilineToWrapConverter()
            : base (TextWrapping.Wrap, TextWrapping.NoWrap)
        {
        }
    }
}
