using Atlante.Presentation.Interfaces;
using System.Collections.ObjectModel;

namespace Atlante.Presentation.Objects
{
    public class ExpanderContainer : IExpanderContainer
    {
        private string _header;

        public ObservableCollection<ExpanderItem> Items { get; private set; }

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public ExpanderContainer( string header )
        {
            this.Header = header;

            this.Items = new ObservableCollection<ExpanderItem>( );
        }

        public void AddItem( string Description, object value )
        {
            this.Items.Add( new ExpanderItem( ) { Description = Description, Value = value } );
        }
    }
}
