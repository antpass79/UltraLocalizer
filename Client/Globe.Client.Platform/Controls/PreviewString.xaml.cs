using Globe.Client.Platform.Models;
using Globe.Client.Platform.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Globe.Client.Platform.Controls
{
    public enum PreviewState
    {
        Valid,
        Invalid
    }

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

        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(PreviewString), new PropertyMetadata(string.Empty));
        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(PreviewString), new PropertyMetadata(true));
        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty PreviewStateProperty =
            DependencyProperty.Register("PreviewState", typeof(PreviewState), typeof(PreviewString), new PropertyMetadata(PreviewState.Valid, OnPreviewStateChanged));
        public PreviewState PreviewState
        {
            get { return (PreviewState)GetValue(PreviewStateProperty); }
            set { SetValue(PreviewStateProperty, value); }
        }
        private static void OnPreviewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var previewString = (PreviewString)d;
            previewString.BorderBrush = previewString.PreviewState == PreviewState.Valid ? Brushes.Green : Brushes.Red;
            previewString.IsValid = previewString.PreviewState == PreviewState.Valid;
        }

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

        public TextBlock InnerTextBlock =>
           innerTextBlock;

        #endregion
    }
}
