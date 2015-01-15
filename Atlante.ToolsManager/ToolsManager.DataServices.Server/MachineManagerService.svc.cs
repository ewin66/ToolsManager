using System.Collections.Generic;
using System.Configuration;
using Atlante.Common;
using ToolsManager.DataServices.Common;
using System;

namespace ToolsManager.DataServices.Server
{
    public class MachineManagerService : IMachineManagerService
    {
        private string _machinesPath;

        private static MachinesDTO _machines;

        public MachineManagerService()
        {
            _machinesPath = ConfigurationManager.AppSettings["machinesPath"];

            if (_machines == null)
                this.LoadData();
        }

        private void LoadData()
        {
            Logger.AddTrace(string.Format("Loading Machines Configuration from: {0}", _machinesPath));

            _machines = Serializer.Deserialize<MachinesDTO>(_machinesPath);

            if (_machines != null)
                return;

            _machines = new MachinesDTO();
        }

        public bool AddMachine(MachineDTO machine)
        {
            Logger.AddTrace("Adding Machine Configuration");

            try
            {
                _machines.Items.Add(machine);
                return true;
            }
            catch (Exception e)
            {
                Logger.AddException(e);
                return false;
            }
        }

        public void DeleteMachine(MachineDTO machine)
        {
            Logger.AddTrace("Deleting Machine Configuration");

            _machines.Items.Remove(machine);
        }

        public void DeleteAllMachines()
        {
            _machines.Items.Clear();
        }

        public List<MachineDTO> GetMachines()
        {
            return _machines.Items;
        }

        public bool SaveChanges()
        {
            Logger.AddTrace(string.Format("Saving {0} Machines", _machines.Items.Count));

            try
            {
                Serializer.Serialize<MachinesDTO>(_machines, _machinesPath);
                return true;
            }
            catch (Exception e)
            {
                Logger.AddException(e);
                return false;
            }
        }
    }
}
