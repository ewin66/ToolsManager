using System.Collections.Generic;
using System.Windows;
using Atlante.Common;

namespace Atlante.Mef.Interfaces
{
    public interface IAddIn : IApplicationInfo
    {
        UIElement CurrentView { get; }

        IList<Command> Commands { get; }
    }
}
