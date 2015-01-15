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
using ToolsManager.DataModel.Common;

namespace ToolsManager.Client.Forms
{
    /// <summary>
    /// Interaction logic for DownloadOptionsForm.xaml
    /// </summary>
    public partial class DownloadOptionsForm : Window
    {
        public DownloadAction SelectedAction { get; set; }

        public DownloadOptionsForm( )
        {
            InitializeComponent( );

            this.SelectedAction = DownloadAction.None;
        }

        private void btnOk_Click( object sender, RoutedEventArgs e )
        {
            if ( radNone.IsChecked.GetValueOrDefault( ) )
                this.SelectedAction = DownloadAction.None;
            else if ( radOverwrite.IsChecked.GetValueOrDefault( ) )
                this.SelectedAction = DownloadAction.Overwrite;
            else if ( radMerge.IsChecked.GetValueOrDefault( ) )
                this.SelectedAction = DownloadAction.Merge;

            this.Close( );
        }
    }
}
