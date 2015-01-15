using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ToolsManager.DataServices.Common
{
    [Serializable]
    public class TaskInfoDTO
    {
        [XmlAttribute( "Id" )]
        public Guid Id { get; set; }

        [XmlAttribute( "Category" )]
        public string Category { get; set; }

        [XmlAttribute( "Description" )]
        public string Description { get; set; }

        public ParametersDTO Parameters { get; set; }

        [XmlElement( "Schedule" )]
        public string Schedule { get; set; }
    }
}
