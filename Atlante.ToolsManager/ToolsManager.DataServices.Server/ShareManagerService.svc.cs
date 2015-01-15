using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Atlante.Common;
using ToolsManager.DataServices.Common;

namespace ToolsManager.DataServices.Server
{
    public class ShareManagerService : IShareManagerService
    {
        private string _sharedTasksPath;

        private static SharedTasksDTO _sharedTasks;

        public ShareManagerService( )
        {
            _sharedTasksPath = ConfigurationManager.AppSettings[ "sharedTasksPath" ];

            if ( _sharedTasks == null )
                this.LoadData( );
        }

        private void LoadData( )
        {
            _sharedTasks = Serializer.Deserialize<SharedTasksDTO>( _sharedTasksPath );

            if ( _sharedTasks != null )
                return;

            _sharedTasks = new SharedTasksDTO( );
        }

        public void ShareTask( string machineSource, string machineTarget, Guid viewId, string viewDescription, TaskInfoDTO task )
        {
            //Find if already exists a task shared from the machine source to machine target.
            var sharedTask = _sharedTasks.Items.Where( t => t.ViewId == viewId && t.MachineSource.ToLower( ) == machineSource.ToLower( ) && t.MachineTarget.ToLower( ) == machineTarget.ToLower( ) && t.Task.Id == task.Id ).FirstOrDefault( );

            if ( sharedTask == null )
                _sharedTasks.Items.Add( new SharedTaskDTO( ) { MachineSource = machineSource, MachineTarget = machineTarget, ViewId = viewId, ViewDescription = viewDescription, Task = task } );
            else
            {
                sharedTask.Task.Category = task.Category;
                sharedTask.Task.Description = task.Description;
                sharedTask.Task.Parameters = task.Parameters;
            }

            Serializer.Serialize<SharedTasksDTO>( _sharedTasks, _sharedTasksPath );
        }

        public void UnshareTask( string machineSource, string machineTarget, Guid viewId, TaskInfoDTO task )
        {
            //Find if already exists a task shared from the machine source to machine target.
            var sharedTask = _sharedTasks.Items.Where( t => t.ViewId == viewId && t.MachineSource.ToLower( ) == machineSource.ToLower( ) && t.MachineTarget.ToLower( ) == machineTarget.ToLower( ) && t.Task.Id == task.Id ).FirstOrDefault( );

            if ( sharedTask == null )
                return;

            _sharedTasks.Items.Remove( sharedTask );

            Serializer.Serialize<SharedTasksDTO>( _sharedTasks, _sharedTasksPath );
        }

        public List<SharedTaskDTO> GetSharedTasks( string machineTarget )
        {
            return _sharedTasks.Items.Where( t => t.MachineTarget.ToLower( ) == machineTarget.ToLower( ) ).ToList( );
        }

        public int GetSharedTasksCount( string machineTarget )
        {
            return _sharedTasks.Items.Where( t => t.MachineTarget.ToLower( ) == machineTarget.ToLower( ) ).ToList().Count;
        }
    }
}
