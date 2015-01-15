using System.ComponentModel;

namespace Atlante.Common
{
    public enum AppPoolAction
    {
        [Description("Start")]
        Start,

        [Description("Stop")]
        Stop,

        [Description("Recycle")]
        Recycle
    }
}
