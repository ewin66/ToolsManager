using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Atlante.Presentation.Objects;

namespace Atlante.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for SelectableListView.xaml
    /// </summary>
    public partial class SelectableListView : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<SelectableItem> _selectableItems;

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty SelectableItemsSourceProperty = DependencyProperty.Register( "SelectableItemsSource", typeof( IList<string> ), typeof( SelectableListView ), new UIPropertyMetadata( null, OnSelectableItemsChanged ) );
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register( "SelectedItems", typeof( IList<string> ), typeof( SelectableListView ), new UIPropertyMetadata( null, OnSelectedItemsChanged ) );

        public IList<string> SelectedItems
        {
            get { return (IList<string>) GetValue( SelectedItemsProperty ); }
            set { SetValue( SelectedItemsProperty, value ); }
        }

        public IList<string> SelectableItemsSource
        {
            get { return (IList<string>) GetValue( SelectableItemsSourceProperty ); }
            set { SetValue( SelectableItemsSourceProperty, value ); }
        }

        public ObservableCollection<SelectableItem> SelectableItems
        {
            get { return _selectableItems; }
            set
            {
                _selectableItems = value;

                if ( this.PropertyChanged != null )
                    this.PropertyChanged( this, new PropertyChangedEventArgs( "SelectableItems" ) );
            }
        }

        public SelectableListView( )
        {
            this.DataContext = this;

            InitializeComponent( );
        }

        private void LoadSelectableItems( IList<string> selectableItemsSource )
        {
            if ( selectableItemsSource == null )
                return;

            this.SelectableItems = new ObservableCollection<SelectableItem>( );

            foreach ( var item in selectableItemsSource )
                this.SelectableItems.Add( new SelectableItem( ) { Description = item } );

        }

        private void LoadSelectedItems( IList<string> selectedItems )
        {
            if ( this.SelectableItems == null )
                return;

            if ( selectedItems == null )
                return;

            foreach ( var item in this.SelectableItems )
            {
                item.IsSelected = false;

                foreach ( var selectedItem in selectedItems )
                {
                    if ( selectedItem != item.Description )
                        continue;

                    item.IsSelected = true;
                }
            }
        }

        private static void OnSelectedItemsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var selectableView = ( d as SelectableListView );

            if ( selectableView == null )
                return;

            selectableView.LoadSelectableItems( selectableView.SelectableItemsSource );
            selectableView.LoadSelectedItems( selectableView.SelectedItems );
        }

        private static void OnSelectableItemsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var selectableView = ( d as SelectableListView );

            if ( selectableView == null )
                return;

            selectableView.LoadSelectableItems( selectableView.SelectableItemsSource );
        }

        private void CheckBox_Checked( object sender, RoutedEventArgs e )
        {
            this.SelectedItems.Add( ( e.Source as CheckBox ).Content.ToString( ) );
        }

        private void CheckBox_Unchecked( object sender, RoutedEventArgs e )
        {
            this.SelectedItems.Remove( ( e.Source as CheckBox ).Content.ToString( ) );
        }

        private void UserControl_Loaded( object sender, RoutedEventArgs e )
        {
            this.LoadSelectableItems( this.SelectableItemsSource );
            this.LoadSelectedItems( this.SelectedItems );
        }
    }
}
