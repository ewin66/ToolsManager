using Atlante.Common;
using Atlante.Common.Interfaces;
using System.Collections.Generic;
using asyncTasks = System.Threading.Tasks;

namespace Atlante.Mef.Interfaces
{
    public interface ITool
    {
        string ToolName { get; }

        string ToolDescription { get; }

        bool CanShareFiles { get; }

        bool IsAsynchronous { get; }

        void SetParameters( Parameters parameters );

        void SetScreen( IScreen screen );

        IEnumerable<ParameterBase> GetParametersDefinition( );        

        ITool CreateInstance( );

        IMessages Run( );

        asyncTasks.Task<IMessages> RunAsync( );
    }
}
