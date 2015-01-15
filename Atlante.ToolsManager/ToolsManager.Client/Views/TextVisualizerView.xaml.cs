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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolsManager.Client.Views
{
    /// <summary>
    /// Interaction logic for TextVisualizerView.xaml
    /// </summary>
    public partial class TextVisualizerView : UserControl
    {
        public string Value { get; private set; }

        public TextVisualizerView( string value )
        {
            this.DataContext = this;

            this.Value = value;

            InitializeComponent( );
        }
    }
}
