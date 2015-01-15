using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client;

namespace ToolsManager.Client.ViewModels
{
    public class ViewTemplateViewModel
    {
        private ViewTemplates _viewsTemplatesSource;

        public ObservableCollection<ViewTemplate> ViewTemplates { get; private set; }
        public ObservableCollection<string> AvailableTools { get; private set; }

        public ViewTemplate SelectedItem { get; set; }

        public ViewTemplateViewModel( ViewTemplates viewTemplates )
        {
            _viewsTemplatesSource = viewTemplates;

            this.LoadData( );
            this.LoadAvailableTools( );
        }

        private void LoadData( )
        {
            this.ViewTemplates = new ObservableCollection<ViewTemplate>( );

            foreach ( ViewTemplate template in _viewsTemplatesSource.Items.OrderBy( i => i.Description ) )
                this.ViewTemplates.Add( template );

            this.ViewTemplates.CollectionChanged += new NotifyCollectionChangedEventHandler( ViewTemplatesViewCollectionChanged );
            _viewsTemplatesSource.CollectionChanged += new NotifyCollectionChangedEventHandler( ViewTemplatesSourceCollectionChanged );
        }

        private void LoadAvailableTools( )
        {
            this.AvailableTools = new ObservableCollection<string>( );

            foreach ( var category in TasksManager.Create( ).AvailableTools.OrderBy( c => c.ToString( ) ) )
                this.AvailableTools.Add( category );
        }

        private void ViewTemplatesViewCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if ( e.Action == NotifyCollectionChangedAction.Add )
            {
                var viewModel = e.NewItems[ 0 ] as ViewTemplate;

                if ( viewModel == null )
                    return;

                viewModel.Id = Guid.NewGuid( );
                _viewsTemplatesSource.Add( viewModel );
            }

            if ( e.Action == NotifyCollectionChangedAction.Remove )
            {
                var viewModel = e.OldItems[ 0 ] as ViewTemplate;

                if ( viewModel == null )
                    return;

                _viewsTemplatesSource.Remove( viewModel );
            }
        }

        private void ViewTemplatesSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            //if ( e.Action == NotifyCollectionChangedAction.Add )
            //    this.ViewTemplates.Add( (ViewTemplate) e.NewItems[ 0 ] );

            //if ( e.Action == NotifyCollectionChangedAction.Remove )
            //    this.ViewTemplates.Remove( (ViewTemplate) e.OldItems[ 0 ] );

            //if ( e.Action == NotifyCollectionChangedAction.Replace )
            //this.ViewTemplates.Remove( (ViewTemplate) e.OldItems[ 0 ] );
        }
    }
}
