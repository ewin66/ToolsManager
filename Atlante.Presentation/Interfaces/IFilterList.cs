using System;
using System.Collections.Generic;
using Atlante.Presentation.Objects;

namespace Atlante.Presentation.Interfaces
{
    public interface IFilterList
    {
        string FilterProperty { get; }
        IList<SelectableItem> FilterSource { get; }

        event Action OnSelectionChanged;
    }
}
