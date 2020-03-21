using System;
using System.Globalization;
using Xamarin.Forms;

namespace LS_Report.Converters
{
    internal class GpsStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string _value = (string)value;
                if (string.IsNullOrWhiteSpace(_value))
                {
                    return ImageSource.FromFile("gpsr.png");
                }
                else
                {
                    return ImageSource.FromFile("gpsg.png");
                }
            }
            else
            {
                return ImageSource.FromFile("gpsr.png");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}