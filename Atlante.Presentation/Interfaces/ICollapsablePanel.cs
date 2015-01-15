using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlante.Presentation.Interfaces
{
    public interface ICollapsablePanel
    {
        string Header { get; }
        bool Visible { get; }
    }
}
