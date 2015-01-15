using System.Collections.Generic;
using System.ComponentModel;

namespace Atlante.Presentation.Interfaces
{
    public interface IProperty : INotifyPropertyChanged
    {
        string Key { get; set; }
        object Value { get; set; }
        IList<object> Options { get; set; }

        PropertyEditorType EditorType { get; set; }
    }
}
