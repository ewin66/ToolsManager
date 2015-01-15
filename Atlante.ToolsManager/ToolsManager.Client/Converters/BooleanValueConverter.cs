using System;
using System.Globalization;
using System.Windows.Data;

namespace ToolsManager.Client.Converters
{
    public class BooleanValueConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            return System.Convert.ToBoolean( value );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return System.Convert.ToString( value );
        }
    }
}
