using System;
using System.Windows.Data;
using System.Windows.Media;
using Atlante.Common;

namespace ToolsManager.Client.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            SolidColorBrush color;

            if ( value == null )
                color = new SolidColorBrush( Colors.White );

            if ( value.ToString( ) == TaskStatus.Error.ToString( ) )
                color = new SolidColorBrush( Color.FromRgb( 255, 220, 220 ) );
            else if ( value.ToString( ) == TaskStatus.Success.ToString( ) )
                color = new SolidColorBrush( Color.FromRgb( 220, 255, 220 ) );
            else if ( value.ToString( ) == TaskStatus.Running.ToString( ) )
                color = new SolidColorBrush( Color.FromRgb( 255, 240, 220 ) );
            else
                color = new SolidColorBrush( Colors.White );

            return color;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException( );
        }
    }
}
