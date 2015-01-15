using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Atlante.Presentation
{
    public class OpenFolderDialog
    {
        public string FolderName { get; private set; }

        public string InitialDirectory { get; set; }

        public bool? ShowDialog( )
        {
            Window window = new Window( );            
            var dialogView = new FolderDialog( );
            window.Height = dialogView.Height;
            window.Width = dialogView.Width;
            window.Content = dialogView;    
        
            return window.ShowDialog( );
        }
    }
}
