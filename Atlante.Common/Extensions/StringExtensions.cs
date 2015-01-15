using System;
using System.Text;

namespace Atlante.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Replace( this string text, int startIndex, int count, string newText )
        {
            if ( count < 0 )
                return text;

            if ( count != newText.Length )
                return text;

            if ( startIndex + count > text.Length )
                return text;

            StringBuilder builder = new StringBuilder( );

            builder.Append( text.Substring( 0, startIndex ) );
            builder.Append( newText );
            builder.Append( text.Substring( startIndex + count, ( text.Length - ( startIndex + count ) ) ) );

            return builder.ToString( );
        }
    }
}
