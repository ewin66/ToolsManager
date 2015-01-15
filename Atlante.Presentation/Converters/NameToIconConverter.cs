using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Atlante.Presentation.Converters
{
    public class NameToIconConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( value == null )
                return null;

            if ( string.IsNullOrEmpty( value.ToString( ) ) )
                return null;

            if ( value.ToString( ).Contains( "pack" ) )
                return new BitmapImage( new Uri( value.ToString( ), UriKind.Absolute ) );
            else
                return new BitmapImage( new Uri( "/Images/" + value.ToString( ), UriKind.Relative ) );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException( );
        }
    }
}
