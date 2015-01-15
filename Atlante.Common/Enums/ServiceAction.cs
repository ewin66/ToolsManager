using System.ComponentModel;

namespace Atlante.Common
{
    public enum ServiceAction
    {
        [Description("Start")]
        Start,

        [Description("Stop")]
        Stop,

        [Description("Restart")]
        Restart
    }
}
