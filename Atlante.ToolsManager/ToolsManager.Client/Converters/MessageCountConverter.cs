using Atlante.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace ToolsManager.Client.Converters
{
    public class MessageCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var messages = (value as IEnumerable<Message>);

            if (messages == null)
                return "0";

            int messageType = 0;
            Int32.TryParse(parameter.ToString(), out messageType);

            return messages.Where(m => (int)m.Type == messageType).Count().ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
