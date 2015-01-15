using Microsoft.Web.Administration;
using System;
using System.ServiceProcess;

namespace Atlante.Common
{
    public static class ServiceUtilities
    {
        public static IMessages StartService( string serviceName, int timeoutMilliseconds )
        {
            Messages messages = new Messages( );

            ServiceController serviceController = new ServiceController( serviceName );
            try
            {
                if ( serviceController.Status == ServiceControllerStatus.Running )
                    return messages;

                serviceController.Start( );

                serviceController.WaitForStatus( ServiceControllerStatus.Running, TimeSpan.FromMilliseconds( timeoutMilliseconds ) );
            }
            catch ( Exception e )
            {
                messages.AddException( e );
            }
            finally
            {
                serviceController.Close( );
            }

            return messages;
        }

        public static IMessages StopService( string serviceName, int timeoutMilliseconds )
        {
            Messages messages = new Messages( );

            ServiceController serviceController = new ServiceController( serviceName );
            try
            {
                if ( serviceController.Status == ServiceControllerStatus.Stopped )
                    return messages;

                serviceController.Stop( );

                serviceController.WaitForStatus( ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds( timeoutMilliseconds ) );
            }
            catch ( Exception e )
            {
                messages.AddException( e );
            }
            finally
            {
                serviceController.Close( );
            }

            return messages;
        }

        public static IMessages RestartService( string serviceName, int timeoutMilliseconds )
        {
            Messages messages = new Messages( );

            ServiceController serviceController = new ServiceController( serviceName );
            try
            {
                if ( serviceController.Status != ServiceControllerStatus.Running )
                {
                    serviceController.Start( );
                    serviceController.WaitForStatus( ServiceControllerStatus.Running, TimeSpan.FromMilliseconds( timeoutMilliseconds ) );
                }

                serviceController.Stop( );
                serviceController.WaitForStatus( ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds( timeoutMilliseconds ) );

                serviceController.Start( );
                serviceController.WaitForStatus( ServiceControllerStatus.Running, TimeSpan.FromMilliseconds( timeoutMilliseconds ) );
            }
            catch ( Exception e )
            {
                messages.AddException( e );
            }
            finally
            {
                serviceController.Close( );
            }

            return messages;
        }

        public static IMessages StartAppPool(string appPoolName)
        {
            Messages messages = new Messages();

            try
            {
                ServerManager serverManager = new ServerManager();
                Site site = serverManager.Sites[0]; // get site by Index or by siteName
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];

                appPool.Start();
                serverManager.CommitChanges();
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }

        public static IMessages StopAppPool(string appPoolName)
        {
            Messages messages = new Messages();

            try
            {
                ServerManager serverManager = new ServerManager();
                Site site = serverManager.Sites[0]; // get site by Index or by siteName
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];

                appPool.Stop();
                serverManager.CommitChanges();
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }

        public static IMessages RecycleAppPool(string appPoolName)
        {
            Messages messages = new Messages();

            try
            {
                ServerManager serverManager = new ServerManager();
                Site site = serverManager.Sites[0]; // get site by Index or by siteName
                ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];

                appPool.Recycle();
                serverManager.CommitChanges();
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }
    }
}
