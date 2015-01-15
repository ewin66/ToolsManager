using Atlante.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;

namespace ToolsManager.Client.Converters
{
    public class MessageListConverter : IMultiValueConverter
    {
        public object Convert( object[ ] values, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if ( values[ 1 ] == null )
                return null;

            var allMessages = values[ 0 ] as IEnumerable;
            var messageType = (MessageType) values[ 1 ];

            if ( allMessages == null )
                return null;

            List<Message> messages = new List<Message>( );

            foreach ( Message message in allMessages )
                if ( message.Type == messageType )
                    messages.Add( message );

            return messages;
        }

        public object[ ] ConvertBack( object value, Type[ ] targetTypes, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException( );
        }
    }

}
