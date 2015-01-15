using System.ComponentModel;

namespace Atlante.Common
{
    public enum ParameterType
    {
        [Description( "Boolean" )]
        Boolean,

        [Description( "Collection" )]
        Collection,

        [Description( "Decimal" )]
        Decimal,

        [Description( "File Path" )]
        FilePath,

        [Description( "Folder Path" )]
        FolderPath,

        [Description( "Integer" )]
        Integer,
        
        [Description( "Password" )]
        Password,
        
        [Description( "String" )]
        String,
    }
}
