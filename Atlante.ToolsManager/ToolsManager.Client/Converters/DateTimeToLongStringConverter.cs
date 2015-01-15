using System;
using System.Globalization;
using System.Windows.Data;

namespace ToolsManager.Client.Converters
{
    public class DateTimeToLongStringConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            DateTime date;

            if ( !DateTime.TryParse( value.ToString( ), out date ) )
                return string.Empty;

            if ( date == DateTime.MinValue )
                return string.Empty;

            return date.ToString( "dd/MM/yyyy HH:mm:ss" );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException( );
        }
    }
}
