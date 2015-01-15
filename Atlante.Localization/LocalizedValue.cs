using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Atlante.Localization
{
    internal class LocalizedValue : IServiceProvider, IProvideValueTarget
    {
        public string Format { get; private set; }
        public ReferenceProperty Property { get; private set; }
        public IEnumerable<Binding> Parameters { get; private set; }

        public LocalizedValue( ReferenceProperty property, string format, IEnumerable<Binding> parameters )
        {
            this.Property = property;
            this.Parameters = parameters;
            this.Format = format;
        }

        public object GetValue( )
        {
            var obj = Property.Object;
            if ( obj == null )
                return null;

            var binding = new MultiBinding( ) { StringFormat = this.Format, Mode = BindingMode.OneWay, Converter = null };

            foreach ( var param in Parameters )
            {
                if ( param != null )
                    binding.Bindings.Add( param );
            }

            return binding.ProvideValue( this );
        }

        object IServiceProvider.GetService( Type serviceType )
        {
            if ( serviceType == typeof( IProvideValueTarget ) )
                return this;

            return null;
        }

        object IProvideValueTarget.TargetObject
        {
            get { return Property.Object; }
        }

        object IProvideValueTarget.TargetProperty
        {
            get { return (DependencyProperty) Property.Property; }
        }
    }
}
