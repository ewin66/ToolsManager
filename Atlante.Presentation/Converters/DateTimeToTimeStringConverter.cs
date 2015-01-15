using System;
using System.Globalization;
using System.Windows.Data;

namespace Atlante.Presentation.Converters
{
    public class DateTimeToTimeStringConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            DateTime date;

            if ( !DateTime.TryParse( value.ToString( ), out date ) )
                return string.Empty;

            if ( date == DateTime.MinValue )
                return string.Empty;

            return date.ToString( "HH:mm:ss" );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException( );
        }
    }
}
