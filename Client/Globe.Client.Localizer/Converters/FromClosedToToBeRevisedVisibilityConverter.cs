using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Globe.Client.Localizer.Converters
{
    class FromClosedToToBeRevisedVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var jobListStatus = (string)values[0];
            var isMasterTranslator = (bool)values[1];


            bool visible = false;
            
            if(jobListStatus == "Closed")
            {
                if (isMasterTranslator)
                    visible = true;
            }
                
            if (visible)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}