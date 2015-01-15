using System;
using System.Xml.Serialization;

namespace ToolsManager.DataServices.Common
{
    [Serializable]
    public class ParameterDTO
    {
        [XmlAttribute( "Key" )]
        public string Key { get; set; }

        [XmlAttribute( "Value" )]
        public string Value { get; set; }

        [XmlAttribute( "Default" )]
        public string DefaultValue { get; set; }

        [XmlAttribute( "IsSystem" )]
        public bool IsSystem { get; set; }

        [XmlAttribute( "Type" )]
        public string ParameterType { get; set; }
    }
}
