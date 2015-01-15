using System;
using System.Xml.Serialization;

namespace ToolsManager.DataServices.Common
{
    [Serializable]
    [XmlRoot( "Machine" )]
    public class MachineDTO
    {
        [XmlAttribute( "Name" )]
        public string Name { get; set; }

        [XmlAttribute( "Owner" )]
        public string Owner { get; set; }

        public MachineDTO( )
        {
            //Nothing to do
        }
    }
}
