using Atlante.Presentation.Interfaces;
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

namespace Atlante.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for PropertiesEditor.xaml
    /// </summary>
    public partial class PropertiesEditor : UserControl
    {
        public IProperty SelectedItem { get; private set; }

        public PropertiesEditor()
        {
            InitializeComponent();
        }

        private void PropertyCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
                return;

            var editor = (this.DataContext as IPropertiesEditor);

            if (editor == null)
                return;

            editor.PropertyValueChanged(this.SelectedItem.Key, (sender as CheckBox).IsChecked);
        }

        private void PropertyTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.SelectedItem == null)
                return;

            var editor = (this.DataContext as IPropertiesEditor);

            if (editor == null)
                return;

            editor.PropertyValueChanged(this.SelectedItem.Key, (sender as TextBox).Text);
        }

        private void PropertySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                this.SelectedItem = null;

            if (e.AddedItems.Count != 1)
                return;

            this.SelectedItem = e.AddedItems[0] as IProperty;
        }
    }
}
