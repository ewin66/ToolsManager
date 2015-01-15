using System;
using System.Collections.ObjectModel;

namespace Atlante.Presentation.Interfaces
{
    public interface IExpanderList
    {
        event Action<object> OnSelectionChanged;

        ObservableCollection<IExpanderContainer> Expanders { get; }

        IExpanderContainer AddExpander( string header );
    }
}
