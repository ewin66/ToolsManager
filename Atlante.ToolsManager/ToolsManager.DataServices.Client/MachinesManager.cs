using System;
using Atlante.Common;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client.MachineServiceProxy;
using ToolsManager.DataServices.Common;

namespace ToolsManager.DataServices.Client
{
    public class MachinesManager
    {
        public Machines Machines { get; private set; }

        public MachinesManager()
        {
            this.LoadData();
        }

        private void LoadData()
        {
            Logger.AddTrace("Loading Machines Configuration");

            this.Machines = new Machines();

            MachineManagerServiceClient proxy = new MachineManagerServiceClient();

            try
            {
                MachineDTO[] machines = proxy.GetMachines();

                foreach (MachineDTO machineDTO in machines)
                {
                    ObjectMapper mapper = new ObjectMapper(typeof(Machine), typeof(MachineDTO));
                    Machine machine = mapper.Map(machineDTO, true) as Machine;

                    this.Machines.Add(machine);
                }
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }
            finally
            {
                proxy.Close();
            }
        }

        public void SaveChanges()
        {
            Logger.AddTrace("Saving Machines Configuration");

            MachineManagerServiceClient proxy = new MachineManagerServiceClient();

            try
            {
                proxy.DeleteAllMachines();

                foreach (Machine machine in this.Machines.Items)
                {
                    ObjectMapper mapper = new ObjectMapper(typeof(Machine), typeof(MachineDTO));
                    MachineDTO machineDTO = mapper.Map(machine, false) as MachineDTO;

                    bool machineSaved = proxy.AddMachine(machineDTO);

                    Logger.AddTrace(string.Format("Machine {0} saved: {1}", machine.Name, machineSaved));
                }

                bool saved = proxy.SaveChanges();

                Logger.AddTrace(string.Format("Machines saved: {0}", saved));
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }
            finally
            {
                proxy.Close();
            }
        }
    }
}
