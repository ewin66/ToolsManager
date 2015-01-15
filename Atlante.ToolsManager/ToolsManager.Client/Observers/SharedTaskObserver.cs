using System;
using System.Windows.Threading;
using ToolsManager.DataServices.Client;

namespace ToolsManager.Client.Observers
{
    public static class SharedTaskObserver
    {
        private static DispatcherTimer _timer;

        public static void Start( )
        {
            _timer = new DispatcherTimer( );
            _timer.Interval = TimeSpan.FromSeconds( 300 ); //5 min.
            _timer.Tick += new EventHandler( OnTimerElapsed );

            _timer.Start( );
        }

        private static void OnTimerElapsed( object sender, EventArgs e )
        {
            AppInfo.NotifyPendingSharedTasks( ViewsManager.Create( ).HasPendingSharedTask( ) );
            AppInfo.NotifyChanges( );
        }
    }
}
