using System;
using System.ComponentModel;
using System.Linq;

namespace Atlante.Presentation.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription( this Enum enumValue )
        {
            var descriptionAttribute = enumValue.GetType( )
              .GetField( enumValue.ToString( ) )
              .GetCustomAttributes( typeof( DescriptionAttribute ), false )
              .FirstOrDefault( ) as DescriptionAttribute;

            return descriptionAttribute != null
              ? descriptionAttribute.Description
              : enumValue.ToString( );
        }
    }
}
