using Globe.Client.Platform.Models;
using Globe.Client.Platform.Services;
using System.Windows;
using System.Windows.Controls;

namespace Globe.Client.Platform.Controls
{
    /// <summary>
    /// Interaction logic for StringPreview.xaml
    /// </summary>
    public partial class PreviewString : UserControl
    {       
        #region Constructors

        public PreviewString()
        {
            InitializeComponent();
        }

        #endregion       

        #region Dependency Properties

        public static readonly DependencyProperty PreviewStyleInfoProperty =
            DependencyProperty.Register("PreviewStyleInfo", typeof(PreviewStyleInfo), typeof(PreviewString), new PropertyMetadata(PreviewStyleInfo.Default()));
        public PreviewStyleInfo PreviewStyleInfo
        {
            get { return (PreviewStyleInfo)GetValue(PreviewStyleInfoProperty); }
            set { SetValue(PreviewStyleInfoProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PreviewString), new PropertyMetadata(string.Empty));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public TextBox InnerTextBox =>
           innerTextBox;

        #endregion
    }
}
