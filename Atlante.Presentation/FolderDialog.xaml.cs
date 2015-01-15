using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace Atlante.Presentation
{
    /// <summary>
    /// Interaction logic for FolderDialog.xaml
    /// </summary>
    public partial class FolderDialog : UserControl
    {
        public ObservableCollection<DriveInfo> Drives { get; private set; }

        public FolderDialog( )
        {
            this.DataContext = this;

            this.LoadDrives( );

            InitializeComponent( );
        }

        private void LoadDrives( )
        {
            this.Drives = new ObservableCollection<DriveInfo>( );

            foreach ( var drive in DriveInfo.GetDrives( ) )
                if ( drive.IsReady )
                    this.Drives.Add( drive );
        }
    }
}
