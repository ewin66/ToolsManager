using System;
using System.Windows;
using System.Windows.Input;
using Atlante.Presentation.Interfaces;
using Atlante.Presentation.Utils;
using System.Diagnostics;

namespace Atlante.Presentation.Behaviors
{
    public class DragDataBehabior : DependencyObject
    {
        private static Type ObjectType;

        public static readonly DependencyProperty IsDragEnabledProperty = DependencyProperty.RegisterAttached( "IsDragEnabled", typeof( bool ), typeof( DragDataBehabior ), new PropertyMetadata( false, OnIsDragEnabledChanged ) );
        public static readonly DependencyProperty IsDropEnabledProperty = DependencyProperty.RegisterAttached( "IsDropEnabled", typeof( bool ), typeof( DragDataBehabior ), new PropertyMetadata( false, OnIsDropEnabledChanged ) );

        public static bool GetIsDragEnabled( DependencyObject element )
        {
            return (bool) element.GetValue( IsDragEnabledProperty );
        }

        public static void SetIsDragEnabled( DependencyObject element, bool isDragEnabled )
        {
            element.SetValue( IsDragEnabledProperty, isDragEnabled );
        }

        public static void OnIsDragEnabledChanged( DependencyObject element, DependencyPropertyChangedEventArgs args )
        {
            var source = element as FrameworkElement;

            source.MouseLeftButtonDown += new MouseButtonEventHandler( Source_MouseLeftButtonDown );
        }

        public static bool GetIsDropEnabled( DependencyObject element )
        {
            return (bool) element.GetValue( IsDropEnabledProperty );
        }

        public static void SetIsDropEnabled( DependencyObject element, bool isDropEnabled )
        {
            element.SetValue( IsDropEnabledProperty, isDropEnabled );
        }

        public static void OnIsDropEnabledChanged( DependencyObject element, DependencyPropertyChangedEventArgs args )
        {
            var target = element as FrameworkElement;

            target.AllowDrop = true;
            target.Drop += new DragEventHandler( Target_Drop );
        }

        private static void Source_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            IDragData source = TreeFinder.FindInterface<IDragData>( sender as FrameworkElement );

            if ( source == null )
                Debug.Assert( false, "In order to complete drag operation IDragData must be implement by the source." );

            var dragObject = source.GetDragObject( );
            ObjectType = dragObject.GetType( );

            DragDrop.DoDragDrop( sender as DependencyObject, dragObject, DragDropEffects.Copy );
        }

        private static void Target_Drop( object sender, DragEventArgs e )
        {
            var targetElement = sender as FrameworkElement;
            var targetModel = TreeFinder.FindInterface<IDragData>( targetElement );

            if ( targetModel == null )
                Debug.Assert( false, "In order to complete drag operation IDragData must be implement by the source." );

            Point pos = e.GetPosition( targetElement );

            targetModel.SetDropObject( e.Data.GetData( ObjectType ), (int) pos.X, (int) pos.Y );
        }
    }
}
