using System.Windows;
using ToolsManager.Client.Forms;
using Atlante.Common.Interfaces;

namespace ToolsManager.Client.ViewModels
{
    public class BaseViewModel : IScreen
    {
        public virtual string Title { get { return string.Empty; } }

        public bool ShowDialog( string title, object content )
        {
            var element = content as FrameworkElement;

            if ( element == null )
                return false;

            ContentForm form = new ContentForm( title, element );
            form.Owner = Application.Current.MainWindow;
            form.ShowDialog( );

            return form.DialogResult.GetValueOrDefault( );
        }
    }
}
