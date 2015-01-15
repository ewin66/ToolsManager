using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlante.Presentation.Interfaces
{
    public interface ITextFinder
    {
        string Text { get; }

        event EventHandler FindNext;
        event EventHandler FindPrevious;
    }
}
