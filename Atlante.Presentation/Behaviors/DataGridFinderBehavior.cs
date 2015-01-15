using System;
using System.Windows;
using System.Windows.Controls;
using Atlante.Presentation.Interfaces;

namespace Atlante.Presentation.Behaviors
{
    public class DataGridFinderBehavior : DependencyObject
    {
        private DataGrid _dataGrid;
        private ITextFinder _textFinder;

        public static readonly DependencyProperty FinderControlProperty = DependencyProperty.RegisterAttached( "FinderControl", typeof( ITextFinder ), typeof( DataGridFinderBehavior ), new FrameworkPropertyMetadata( false, OnPropertyAttached ) );

        public static bool GetFinderControl( DependencyObject element )
        {
            return (bool) element.GetValue( FinderControlProperty );
        }

        public static void FinderControlEnabled( DependencyObject element, bool value )
        {
            element.SetValue( FinderControlProperty, value );
        }

        private static void OnPropertyAttached( DependencyObject element, DependencyPropertyChangedEventArgs args )
        {
            new DataGridFinderBehavior( element as DataGrid );
        }

        private DataGridFinderBehavior( DataGrid dataGrid )
        {
            _dataGrid = dataGrid;

            _textFinder.FindNext += new EventHandler( OnFindNext );
            _textFinder.FindPrevious += new EventHandler( OnFindPrevious );
        }

        private void OnFindNext( object sender, EventArgs args )
        {

        }

        private void OnFindPrevious( object sender, EventArgs args )
        {

        }
    }
}
