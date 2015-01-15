using System;
using Atlante.Common;
using Atlante.Common.Interfaces;
using Atlante.Mef.Interfaces;
using asyncTasks = System.Threading.Tasks;

namespace ToolsManager.Tools
{
    public abstract class ToolBase
    {
        protected IScreen Screen { get; private set; }        

        public abstract IMessages Run();

        public void SetScreen(IScreen screen)
        {
            this.Screen = screen;            
        }

        public ITool CreateInstance()
        {
            return (ITool)Activator.CreateInstance(this.GetType());
        }

        public async asyncTasks.Task<IMessages> RunAsync()
        {
            return await asyncTasks.Task.Factory.StartNew(() => { return this.Run(); });
        }
    }
}
