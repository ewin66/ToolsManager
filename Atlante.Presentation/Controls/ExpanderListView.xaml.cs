using System.Windows.Controls;
using Atlante.Presentation.Objects;

namespace Atlante.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for ExpanderListView.xaml
    /// </summary>
    public partial class ExpanderListView : UserControl
    {
        public ExpanderListView( )
        {
            InitializeComponent( );
        }

        private void ListView_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if ( e.AddedItems.Count <= 0 )
                return;

            ( this.DataContext as ExpanderList ).ChangeSelectedItem( ( e.AddedItems[ 0 ] as ExpanderItem ).Value );
        }
    }
}
