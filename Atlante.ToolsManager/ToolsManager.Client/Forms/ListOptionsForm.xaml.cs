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
using System.ComponentModel;

namespace ToolsManager.Client.Forms
{
    /// <summary>
    /// Interaction logic for ListOptionsForm.xaml
    /// </summary>
    public partial class ListOptionsForm : Window, INotifyPropertyChanged
    {
        public string _selectedValue;

        public string Description { get; private set; }

        public IList<string> Options { get; private set; }

        public string SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;

                if ( PropertyChanged != null )
                    PropertyChanged( this, new PropertyChangedEventArgs( "SelectedValue" ) );
            }
        }

        public ListOptionsForm( string description, IList<string> options )
        {
            this.DataContext = this;

            this.Description = description;
            this.Options = options;

            InitializeComponent( );
        }

        private void button1_Click( object sender, RoutedEventArgs e )
        {
            this.DialogResult = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
