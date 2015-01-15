using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Collections.Generic;

namespace Atlante.Presentation.Interfaces
{
    public interface IStatusInfo : IApplicationInfo
    {
        IList<AppLibrary> AppLibraries { get; }

        ActionType Action { get; }
        string ActionDescription { get; }
        bool ActionInProgress { get; }
    }
}
