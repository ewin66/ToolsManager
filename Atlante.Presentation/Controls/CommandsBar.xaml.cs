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
using Atlante.Common;
using System.Collections;

namespace Atlante.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for CommandsBar.xaml
    /// </summary>
    public partial class CommandsBar : UserControl
    {
        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register( "IconHeight", typeof( int ), typeof( CommandsBar ), new UIPropertyMetadata( 24 ) );
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register( "IconWidth", typeof( int ), typeof( CommandsBar ), new UIPropertyMetadata( 24 ) );
        public static readonly DependencyProperty CommandsProperty = DependencyProperty.Register( "CommandsSource", typeof( IList<CommandBase> ), typeof( CommandsBar ), new UIPropertyMetadata( null ) );

        public int IconHeight
        {
            get { return (int) GetValue( IconHeightProperty ); }
            set { SetValue( IconHeightProperty, value ); }
        }

        public int IconWidth
        {
            get { return (int) GetValue( IconWidthProperty ); }
            set { SetValue( IconWidthProperty, value ); }
        }

        public IList<CommandBase> CommandsSource
        {
            get { return (IList<CommandBase>) GetValue( CommandsProperty ); }
            set { SetValue( CommandsProperty, value ); }
        }

        public CommandsBar( )
        {
            this.DataContext = this;

            InitializeComponent( );
        }
    }
}
