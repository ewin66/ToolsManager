using System.ComponentModel;

namespace Atlante.Common
{
    public enum TaskStatus
    {
        [Description("None")]
        None = 0,

        [Description("Stopped")]
        Stopped = 1,

        [Description( "Running" )]
        Running = 2,

        [Description( "Paused" )]
        Paused = 3,

        [Description( "Error" )]
        Error = 4,

        [Description( "Success" )]
        Success = 5 
    }
}
