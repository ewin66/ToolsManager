using System;
using System.Globalization;
using System.Windows.Data;

namespace Atlante.Presentation.Converters
{
    public class BooleanValueConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return System.Convert.ToBoolean( value );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return System.Convert.ToString( value );
        }
    }
}
