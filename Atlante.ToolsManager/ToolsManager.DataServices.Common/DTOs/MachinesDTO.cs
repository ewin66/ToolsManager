using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ToolsManager.DataServices.Common
{
    [Serializable]
    [XmlRoot( "Machines" )]
    public class MachinesDTO
    {
        [XmlArrayItem( "Machine" )]
        public List<MachineDTO> Items { get; set; }

        public MachinesDTO( )
        {
            this.Items = new List<MachineDTO>( );
        }
    }
}
