using System.Windows;
using System.Windows.Controls;
using Atlante.Common;

namespace Atlante.Presentation.Selectors
{
    public class CommandDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate( object item, DependencyObject container )
        {
            var element = container as FrameworkElement;

            if ( element == null )
                return null;

            if ( item == null )
                return null;

            if ( item.GetType( ) == typeof( Command ) )
                return element.FindResource( "commandButtonTemplate" ) as DataTemplate;
            else if ( item.GetType( ) == typeof( CommandSeparator ) )
                return element.FindResource( "commandSeparatorTemplate" ) as DataTemplate;
            else
                return null;
        }
    }
}
