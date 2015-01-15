using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ToolsManager.Client.Converters
{
    public class MessageIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return null;

            return new BitmapImage(new Uri("/Images/" + value.ToString() + ".png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
