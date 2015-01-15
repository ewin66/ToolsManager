using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlante.Presentation.Objects
{
    public class SelectableItem
    {
        public object Value { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
    }
}
