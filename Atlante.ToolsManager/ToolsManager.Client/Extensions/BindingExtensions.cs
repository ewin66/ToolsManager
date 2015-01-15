using System.Windows;
using System.Windows.Data;

namespace ToolsManager.Client
{
    public static class BindingExtensions
    {
        public static void UpdateTarget( this FrameworkElement element, DependencyProperty property )
        {
            BindingExpression expression = element.GetBindingExpression( property );

            if ( expression != null )
            {
                expression.UpdateTarget( );
                return;
            }

            MultiBindingExpression multiExpression = BindingOperations.GetMultiBindingExpression( element, property );

            if ( multiExpression != null )
                multiExpression.UpdateTarget( );
        }

        public static void UpdateSource( this FrameworkElement element, DependencyProperty property )
        {
            BindingExpression expression = element.GetBindingExpression( property );

            if ( expression == null )
                return;

            expression.UpdateSource( );
        }
    }

}
