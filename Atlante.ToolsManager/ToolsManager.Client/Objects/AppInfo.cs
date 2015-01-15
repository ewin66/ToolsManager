using Atlante.Presentation;

namespace ToolsManager.Client
{
    public delegate void ApplicationInfoChangedEventHandler( );

    public static class AppInfo
    {
        public static ActionType Action { get; private set; }
        public static bool ActionInProgress { get; private set; }
        public static double ActionElapsedTime { get; private set; }

        public static bool PendingSharedTasks { get; private set; }

        public static event ApplicationInfoChangedEventHandler ApplicationInfoChanged;

        public static void BeginAction( ActionType action )
        {
            Action = action;
            ActionInProgress = true;

            NotifyChanges( );
        }

        public static void EndAction( ActionType action )
        {
            Action = action;
            ActionInProgress = false;

            NotifyChanges( );
        }

        public static void NotifyPendingSharedTasks( bool hasPendingSharedTasks )
        {
            PendingSharedTasks = hasPendingSharedTasks;
            NotifyChanges( );
        }

        public static void NotifyChanges( )
        {
            if ( ApplicationInfoChanged != null )
                ApplicationInfoChanged( );
        }
    }
}
