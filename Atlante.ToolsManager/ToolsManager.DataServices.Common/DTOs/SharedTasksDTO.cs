using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ToolsManager.DataServices.Common
{
    [Serializable]
    [XmlRoot( "SharedTasks" )]
    public class SharedTasksDTO
    {
        [XmlArrayItem( "SharedTask" )]
        public List<SharedTaskDTO> Items { get; set; }

        public SharedTasksDTO( )
        {
            this.Items = new List<SharedTaskDTO>( );
        }
    }
}
