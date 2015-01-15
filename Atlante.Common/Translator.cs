using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlante.Common
{
    public static class Translator
    {
        public static string Language { get; set; }

        private static IDictionary _dictionary;

        static Translator( )
        {
            Language = "en-US";
        }

        private static void LoadMessages( )
        {
            try
            {
                var doc = XDocument.Load( string.Format( @"{0}\LocalizedMessages.xml", Language ) );

                _dictionary = new Dictionary<string, string>( );

                foreach ( var element in doc.Root.Elements( "Message" ) )
                    _dictionary.Add( element.Attribute( "Key" ).Value, element.Attribute( "Text" ).Value );
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }
        }

        public static string Translate( string key )
        {
            if ( _dictionary == null )
                LoadMessages( );

            if ( _dictionary == null )
                return key;

            var result = _dictionary[ key ];

            if ( result == null )
                return key;

            return result.ToString( );
        }
    }
}
