using System.Dynamic;
using System.Xml.Linq;

namespace Atlante.Common
{
    public class XmlParser : DynamicObject
    {
        private string _fileName;
        private XElement _element;

        public string this[ string attribute ]
        {
            get
            {
                if ( _element == null )
                    return string.Empty;

                return _element.Attribute( attribute ).Value;
            }
        }

        public XmlParser( )
        {
            //
        }

        public XmlParser( string fileName )
        {
            _fileName = fileName;

            _element = XElement.Load( fileName );
        }

        public XmlParser( XElement element )
        {
            _element = element;
        }

        public void Save( )
        {
            _element.Save( _fileName );
        }

        public override bool TrySetMember( SetMemberBinder binder, object value )
        {
            var node = _element.Element( binder.Name );

            if ( node != null )
                node.SetValue( value );
            else
                _element.Add( new XElement( binder.Name, value ) );

            return true;
        }

        public override bool TryGetMember( GetMemberBinder binder, out object result )
        {
            result = null;

            var node = _element.Element( binder.Name );
            if ( node != null )
                result = new XmlParser( node );

            return ( node != null );
        }

        public XmlParser Create( string name, bool append )
        {
            return this.Create( name, string.Empty, append );
        }

        public XmlParser Create( string name, string value, bool append )
        {
            XElement element;

            if ( append )
            {
                element = new XElement( name, value );
                _element.Add( element );
            }
            else
            {
                element = _element.Element( name );
                if ( element != null )
                    element.Value = value;
                else
                {
                    element = new XElement( name, value );
                    _element.Add( element );
                }
            }

            return new XmlParser( element );
        }

        public bool HasNext( )
        {
            return ( _element.NextNode != null );
        }

        public void MoveNext( )
        {
            _element = _element.NextNode as XElement;
        }

        public override string ToString( )
        {
            if ( _element != null )
                return _element.Value;

            return string.Empty;
        }
    }
}
