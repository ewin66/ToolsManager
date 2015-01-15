using Atlante.Common;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ToolsManager.Client.Selectors
{
    public class EditorDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element == null)
                return null;

            var parameter = item as Parameter;

            if (parameter == null)
                return null;

            string editorKey = string.Format("{0}DataTemplate", parameter.ParameterType.ToString());

            var dataTemplate = element.FindResource(editorKey) as DataTemplate;

            if (dataTemplate == null)
                Debug.Assert(false, string.Format("There is no editor template for {0}", editorKey));

            return dataTemplate;
        }
    }
}
