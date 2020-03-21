using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;

namespace LS_Report.Converters
{
    internal class ListViewHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Models.tmp_presented_products_doctor>)
            {
                ObservableCollection<Models.tmp_presented_products_doctor> _value = (ObservableCollection<Models.tmp_presented_products_doctor>)value;
                return 100 * (_value.Count + 1);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}