using System;
using System.Collections.Generic;
using System.ServiceModel;
using ToolsManager.DataServices.Common;

namespace ToolsManager.DataServices.Server
{
    [ServiceContract]
    public interface IShareManagerService
    {
        [OperationContract]
        void ShareTask( string machineSource, string machineTarget, Guid viewId, string viewDescription, TaskInfoDTO task );

        [OperationContract]
        void UnshareTask( string machineSource, string machineTarget, Guid viewId, TaskInfoDTO task );

        [OperationContract]
        List<SharedTaskDTO> GetSharedTasks( string machineTarget );

        [OperationContract]
        int GetSharedTasksCount( string machineTarget );
    }
}
