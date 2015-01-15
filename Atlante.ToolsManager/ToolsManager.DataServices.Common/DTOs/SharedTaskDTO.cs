using System;
using System.Xml.Serialization;

namespace ToolsManager.DataServices.Common
{
    [Serializable]
    [XmlRoot( "SharedTask" )]
    public class SharedTaskDTO
    {
        [XmlAttribute( "Source" )]
        public string MachineSource { get; set; }

        [XmlAttribute( "Target" )]
        public string MachineTarget { get; set; }

        [XmlAttribute( "ViewId" )]
        public Guid ViewId { get; set; }

        [XmlAttribute( "ViewDescription" )]
        public string ViewDescription { get; set; }

        public TaskInfoDTO Task { get; set; }
    }
}
