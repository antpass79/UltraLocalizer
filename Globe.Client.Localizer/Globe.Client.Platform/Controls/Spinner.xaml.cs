using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Globe.Client.Platform.Controls
{
    /// <summary>
    /// Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl
    {
        #region Constructors

        public Spinner()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty BusyProperty =
            DependencyProperty.Register("Busy", typeof(bool), typeof(Spinner), new PropertyMetadata(false));
        public bool Busy
        {
            get { return (bool)GetValue(BusyProperty); }
            set { SetValue(BusyProperty, value); }
        }

        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register("Diameter", typeof(int), typeof(Spinner), new PropertyMetadata(100, OnDiameterPropertyChanged));
        public int Diameter
        {
            get { return (int)GetValue(DiameterProperty); }
            set
            {
                if (value < 10)
                    value = 10;
                SetValue(DiameterProperty, value);
            }
        }
        private static void OnDiameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = (Spinner)d;
            d.CoerceValue(CenterProperty);
            d.CoerceValue(RadiusProperty);
            d.CoerceValue(InnerRadiusProperty);
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(Spinner), new PropertyMetadata(15, null, OnCoerceRadius));
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        private static object OnCoerceRadius(DependencyObject d, object baseValue)
        {
            var control = (Spinner)d;
            int newRadius = (int)(control.GetValue(DiameterProperty)) / 2;
            return newRadius;
        }

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(int), typeof(Spinner), new PropertyMetadata(2, null, OnCoerceInnerRadius));
        public int InnerRadius
        {
            get { return (int)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }
        private static object OnCoerceInnerRadius(DependencyObject d, object baseValue)
        {
            var control = (Spinner)d;
            int newInnerRadius = (int)(control.GetValue(DiameterProperty)) / 4;
            return newInnerRadius;
        }

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(Spinner), new PropertyMetadata(new Point(15, 15), null, OnCoerceCenter));
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        private static object OnCoerceCenter(DependencyObject d, object baseValue)
        {
            var control = (Spinner)d;
            int newCenter = (int)(control.GetValue(DiameterProperty)) / 2;
            return new Point(newCenter, newCenter);
        }
        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(Color), typeof(Spinner), new PropertyMetadata(Colors.Blue));
        public Color Color1
        {
            get { return (Color)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(Color), typeof(Spinner), new PropertyMetadata(Colors.Transparent));
        public Color Color2
        {
            get { return (Color)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        #endregion
    }
}
