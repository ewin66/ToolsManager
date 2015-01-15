using System.Windows;

namespace Atlante.Presentation.Utils
{
    public static class TreeFinder
    {
        public static T FindInterface<T>( FrameworkElement element )
        {
            if ( element == null )
                return default( T );

            if ( element.DataContext is T )
                return (T) element.DataContext;

            return FindInterface<T>( element.Parent as FrameworkElement );
        }
    }
}
