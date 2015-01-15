using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlante.Presentation.Interfaces
{
    public interface IPropertiesEditor
    {
        IList<IProperty> Properties { get; }

        bool ReadOnlyProperties { get; }

        void PropertyValueChanged(string propertyKey, object newValue);
    }
}
