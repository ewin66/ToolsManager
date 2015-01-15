using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolsManager.Client.Forms
{
    /// <summary>
    /// Interaction logic for ContentForm.xaml
    /// </summary>
    public partial class ContentForm : Window
    {
        private const int HORIZONTAL_TOLERANCE = 28;
        private const int VERTICAL_TOLERANCE = 78;

        public double ContentHeight { get; set; }
        public double ContentWidth { get; set; }
        public FrameworkElement ContentElement { get; set; }

        public ContentForm( string title, FrameworkElement content )
        {
            this.DataContext = this;

            Title = title;

            this.ContentWidth = content.MinWidth + HORIZONTAL_TOLERANCE;
            this.ContentHeight = content.MinHeight + VERTICAL_TOLERANCE;
            this.ContentElement = content;

            if ( this.ContentWidth == 0.0 )
                this.ContentWidth = 500;

            if ( this.ContentHeight == 0.0 )
                this.ContentWidth = 350;

            InitializeComponent( );
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            this.DialogResult = true;
        }
    }
}
