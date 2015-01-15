using System.Windows.Controls;
using ToolsManager.Client.ViewModels;

namespace ToolsManager.Client.Views
{
    /// <summary>
    /// Interaction logic for ToolsView.xaml
    /// </summary>
    public partial class ToolsView : UserControl
    {
        public ToolsView( )
        {
            InitializeComponent( );
        }

        private void ListSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            var viewModel = this.DataContext as ToolsViewModel;
            if ( viewModel == null )
                return;

            if ( e.RemovedItems.Count <= 0 )
                return;

            viewModel.ExecuteSaveViewTemplate( e.RemovedItems[ 0 ] );
        }
    }
}
