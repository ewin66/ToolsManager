using System;
using System.Collections.ObjectModel;
using Atlante.Presentation.Interfaces;

namespace Atlante.Presentation.Objects
{
    public class ExpanderList : IExpanderList
    {
        public event Action<object> OnSelectionChanged;

        public ObservableCollection<IExpanderContainer> Expanders { get; private set; }

        public ExpanderList( )
        {
            this.Expanders = new ObservableCollection<IExpanderContainer>( );
        }

        public IExpanderContainer AddExpander( string header )
        {
            var expander = new ExpanderContainer( header );

            this.Expanders.Add( expander );

            return expander;
        }

        public void ChangeSelectedItem( object item )
        {
            if ( this.OnSelectionChanged != null )
                this.OnSelectionChanged( item );
        }
    }
}
