using System.Collections.Generic;
using System.ComponentModel;
using Atlante.Presentation.Interfaces;

namespace Atlante.Presentation.Objects
{
    public class Property : IProperty
    {
        private string _key;
        private object _value;

        public string Key
        {
            get { return _key; }
            set
            {
                if ( value == _key )
                    return;

                _key = value;
                this.NotifyPropertyChanged( "Key" );
            }
        }

        public object Value
        {
            get { return _value; }
            set
            {
                if ( value == _value )
                    return;

                _value = value;
                this.NotifyPropertyChanged( "Value" );
            }
        }

        public IList<object> Options { get; set; }
        public PropertyEditorType EditorType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged( string propertyName )
        {
            if ( this.PropertyChanged != null )
                this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}
