using System;
using System.Xml.Serialization;

namespace Atlante.Common
{
    [Serializable]
    public class Message
    {
        [XmlAttribute( "Type" )]
        public MessageType Type { get; set; }

        [XmlAttribute( "Description" )]
        public string Description { get; set; }

        [XmlAttribute( "Exception" )]
        public string Exception { get; set; }

        [XmlAttribute( "Trace" )]
        public string Trace { get; set; }

        public Message( )
        {
            //Nothing to do
        }
    }
}
