using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ToolsManager.DataModel.Common;

namespace ToolsManager.Client.ViewModels
{
    public class MachinesViewModel
    {
        private Machines _machines;

        public bool ReadOnly { get; set; }

        public ObservableCollection<Machine> Machines { get; set; }

        public Machine SelectedMachine { get; set; }

        public MachinesViewModel( Machines machines, bool readOnly = false )
        {
            _machines = machines;

            this.ReadOnly = readOnly;

            this.LoadData( );
        }

        private void LoadData( )
        {
            this.Machines = new ObservableCollection<Machine>( );

            foreach ( Machine client in _machines.Items.OrderBy( m => m.Name ) )
                this.Machines.Add( client );

            this.Machines.CollectionChanged += new NotifyCollectionChangedEventHandler( TemplatesCollectionChanged );
        }

        private void TemplatesCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if ( e.Action == NotifyCollectionChangedAction.Add )
                _machines.Add( (Machine) e.NewItems[ 0 ] );

            if ( e.Action == NotifyCollectionChangedAction.Remove )
                _machines.Remove( (Machine) e.OldItems[ 0 ] );
        }
    }
}
