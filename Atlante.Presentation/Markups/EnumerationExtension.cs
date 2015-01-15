using System;
using System.Linq;
using System.Windows.Markup;
using Atlante.Common;
using Atlante.Presentation.Extensions;

namespace Atlante.Presentation.Markups
{
    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumerationExtension( Type enumType )
        {
            if ( enumType == null )
                throw new ArgumentNullException( "enumType" );

            EnumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if ( _enumType == value )
                    return;

                var enumType = Nullable.GetUnderlyingType( value ) ?? value;

                if ( enumType.IsEnum == false )
                    throw new ArgumentException( "Type must be an Enum." );

                _enumType = value;
            }
        }

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            var enumValues = Enum.GetValues( EnumType );

            return (
              from Enum enumValue in enumValues
              select new EnumerationMember
              {
                  Value = (int) ( (object) enumValue ),
                  Description = Translator.Translate( enumValue.GetDescription( ) )
              } ).ToArray( );
        }

        public class EnumerationMember
        {
            public string Description { get; set; }
            public int Value { get; set; }
        }
    }
}