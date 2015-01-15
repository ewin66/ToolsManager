using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Atlante.Presentation.Interfaces;

namespace Atlante.Presentation.Selectors
{
    public class EditorDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate( object item, DependencyObject container )
        {
            var element = container as FrameworkElement;

            if ( element == null )
                return null;

            var property = item as IProperty;

            if ( property == null )
                return null;

            var editorKey = string.Format( "{0}DataTemplate", property.EditorType.ToString( ) );

            var dataTemplate = element.FindResource( editorKey ) as DataTemplate;

            if ( dataTemplate == null )
                Debug.Assert( false, string.Format( "There is no editor template for {0}", editorKey ) );

            return dataTemplate;
        }
    }
}
