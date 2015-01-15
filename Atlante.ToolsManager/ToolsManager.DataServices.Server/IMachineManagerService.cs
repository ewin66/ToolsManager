using System.Collections.Generic;
using System.ServiceModel;
using ToolsManager.DataServices.Common;

namespace ToolsManager.DataServices.Server
{
    [ServiceContract()]
    public interface IMachineManagerService
    {
        [OperationContract()]
        bool AddMachine(MachineDTO machine);

        [OperationContract()]
        void DeleteMachine(MachineDTO machine);

        [OperationContract()]
        void DeleteAllMachines();

        [OperationContract()]
        List<MachineDTO> GetMachines();

        [OperationContract()]
        bool SaveChanges();
    }
}
