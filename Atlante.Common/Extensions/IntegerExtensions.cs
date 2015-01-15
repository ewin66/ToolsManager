
namespace Atlante.Common.Extensions
{
    public static class IntegerExtensions
    {
        public static string CompleteTwoDigits( this int number )
        {
            if ( number < 10 )
                return "0" + number;

            return number.ToString( );
        }
    }
}
