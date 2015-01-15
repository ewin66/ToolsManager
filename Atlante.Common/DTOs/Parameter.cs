using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Atlante.Common
{
    [Serializable]
    public class Parameter : ParameterBase, INotifyPropertyChanged
    {
        private string _value;

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlAttribute( "Value" )]
        public string Value
        {
            get { return _value; }
            set
            {
                if ( _value == value )
                    return;

                _value = value;
                this.NotifyPropertyChanged( "Value" );
            }
        }

        [XmlAttribute( "IsSystem" )]
        public bool IsSystem { get; set; }

        public Parameter( )
            : base( string.Empty, null, ParameterType.String )
        {
            //Nothing to do
        }

        private void NotifyPropertyChanged( string property )
        {
            if ( this.PropertyChanged != null )
                this.PropertyChanged( this, new PropertyChangedEventArgs( property ) );
        }
    }
}
