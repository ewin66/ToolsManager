using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Atlante.Presentation.Behaviors
{
    public class MovementBehavior : DependencyObject
    {
        private FrameworkElement _element;
        private FrameworkElement _container;

        private Point _elementStartPosition;
        private Point _mouseStartPosition;

        private TranslateTransform _transform;

        public static readonly DependencyProperty IsMovementEnabledProperty = DependencyProperty.RegisterAttached( "IsMovementEnabled", typeof( bool ), typeof( MovementBehavior ), new FrameworkPropertyMetadata( false, OnPropertyAttached ) );

        public static bool GetIsMovementEnabled( DependencyObject element )
        {
            return (bool) element.GetValue( IsMovementEnabledProperty );
        }

        public static void SetIsMovementEnabled( DependencyObject element, bool value )
        {
            element.SetValue( IsMovementEnabledProperty, value );
        }

        private static void OnPropertyAttached( DependencyObject element, DependencyPropertyChangedEventArgs args )
        {
            new MovementBehavior( element as FrameworkElement );
        }

        private MovementBehavior( FrameworkElement element )
        {
            _element = element;

            _container = VisualTreeHelper.GetParent( _element as DependencyObject ) as FrameworkElement;
            if ( _container == null )
                return;

            _element.MouseLeftButtonDown += new MouseButtonEventHandler( this.OnMouseLeftButtonDown );
            _element.MouseLeftButtonUp += new MouseButtonEventHandler( this.OnMouseLeftButtonUp );
            _element.MouseMove += new MouseEventHandler( this.OnMouseMove );
        }

        private void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            _mouseStartPosition = e.GetPosition( _container );

            _element.Opacity = 0.2;

            _element.CaptureMouse( );
        }

        private void OnMouseMove( object sender, MouseEventArgs e )
        {
            var mousePosition = e.GetPosition( _container );

            Vector diff = ( mousePosition - _mouseStartPosition );

            if ( !_element.IsMouseCaptured )
                return;

            this.VerifyRenderTransform( );

            _transform.X = _elementStartPosition.X + diff.X;
            _transform.Y = _elementStartPosition.Y + diff.Y;
        }

        private void OnMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            _element.ReleaseMouseCapture( );

            _element.Opacity = 1.0;

            this.VerifyRenderTransform( );

            _elementStartPosition.X = _transform.X;
            _elementStartPosition.Y = _transform.Y;
        }

        private void VerifyRenderTransform( )
        {
            if ( _transform != null )
                return;

            if ( !( _element.RenderTransform is TranslateTransform ) )
                _element.RenderTransform = new TranslateTransform( );

            _transform = _element.RenderTransform as TranslateTransform;
            _elementStartPosition.X = _transform.X;
            _elementStartPosition.Y = _transform.Y;
        }
    }
}