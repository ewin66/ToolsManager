using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlante.Common.Interfaces
{
    public interface IScreen
    {
        bool ShowDialog( string title, object content );
    }
}
