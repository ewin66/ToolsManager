using System.ComponentModel;

namespace Atlante.Common
{
    public enum ScheduleFrequency
    {
        [Description( "UI_ONE_TIME" )]
        Once,

        [Description( "UI_DAILY" )]
        Daily,

        [Description( "UI_WEEKLY" )]
        Weekly
    }
}
