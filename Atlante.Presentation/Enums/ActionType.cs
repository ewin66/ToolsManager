using System.ComponentModel;

namespace Atlante.Presentation
{
    public enum ActionType
    {
        [Description( "UI_ACTION_NONE" )]
        None = 0,

        [Description( "UI_ACTION_SHARE_TASK" )]
        ShareTask = 1,

        [Description( "UI_ACTION_LOAD_DATA" )]
        LoadData = 2,

        [Description( "UI_ACTION_REFRESH_DATA" )]
        RefreshData = 3
    }
}
