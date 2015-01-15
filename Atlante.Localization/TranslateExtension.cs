using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Atlante.Common;

namespace Atlante.Localization
{
    [MarkupExtensionReturnType( typeof( object ) )]
    public class TranslateExtension : MarkupExtension
    {
        public string Key { get; set; }

        public Binding Param1 { get; set; }
        public Binding Param2 { get; set; }
        public Binding Param3 { get; set; }

        public TranslateExtension( )
        {
            //Nothing to do.
        }

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            var service = serviceProvider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;

            if ( service == null )
                return null;

            if ( !( service.TargetObject is DependencyObject ) )
                return null;

            var property = new ReferenceProperty( (DependencyObject) service.TargetObject, (DependencyProperty) service.TargetProperty );

            var parameters = this.GetParameters( );

            var localizedText = Translator.Translate( this.Key );

            if ( parameters.Count == 0 )
                return localizedText;

            var localizedValue = new LocalizedValue( property, localizedText, parameters );

            return localizedValue.GetValue( );
        }

        private Collection<Binding> GetParameters( )
        {
            var parameters = new Collection<Binding>( );

            if ( this.Param1 != null )
                parameters.Add( Param1 );
            if ( this.Param2 != null )
                parameters.Add( Param2 );
            if ( this.Param3 != null )
                parameters.Add( Param3 );

            return parameters;
        }
    }
}
