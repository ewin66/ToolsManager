using System;
using System.ComponentModel;
using System.Windows;

namespace Atlante.Localization
{
    internal class ReferenceProperty
    {
        private WeakReference _object;

        public object Property { get; private set; }

        public bool IsInDesignMode
        {
            get
            {
                var obj = _object.Target as DependencyObject;

                return obj != null && DesignerProperties.GetIsInDesignMode( obj );
            }
        }

        public DependencyObject Object
        {
            get
            {
                return _object.Target as DependencyObject;
            }
        }

        public ReferenceProperty( DependencyObject obj, DependencyProperty property )
        {
            _object = new WeakReference( obj );

            this.Property = property;
        }
    }
}
