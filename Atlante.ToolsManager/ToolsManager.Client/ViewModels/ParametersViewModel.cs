using Atlante.Common;
using Atlante.Presentation;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace ToolsManager.Client.ViewModels
{
    public class ParametersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Parameters _parametersInfo;

        public Command BrowseFilePathCommand { get; set; }
        public Command BrowseFolderPathCommand { get; set; }

        public ObservableCollection<Parameter> Parameters { get; set; }

        public ParametersViewModel(Parameters parameters)
        {
            _parametersInfo = parameters;

            this.LoadParameters();
            this.ConfigureCommands();
        }

        private void LoadParameters()
        {
            this.Parameters = new ObservableCollection<Parameter>();

            foreach (var param in _parametersInfo.Items.OrderBy(p => p.Key))
                this.Parameters.Add(param);
        }

        private void ConfigureCommands()
        {
            this.BrowseFilePathCommand = new Command("Browse", this.ExecuteBrowseFilePath);
            this.BrowseFolderPathCommand = new Command("Browse", this.ExecuteBrowseFolderPath);
        }

        private void ExecuteBrowseFilePath(object parameter)
        {
            var filePath = parameter as TextBox;

            if (filePath == null)
                return;

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.CheckFileExists = false;
            openDialog.CheckPathExists = true;
            openDialog.InitialDirectory = filePath.Text;

            if (!openDialog.ShowDialog().GetValueOrDefault())
                return;

            filePath.Text = openDialog.FileName;
        }

        private void ExecuteBrowseFolderPath(object parameter)
        {
            var folderPath = parameter as TextBox;

            if (folderPath == null)
                return;

            OpenFolderDialog openDialog = new OpenFolderDialog();
            openDialog.InitialDirectory = folderPath.Text;

            if (!openDialog.ShowDialog().GetValueOrDefault())
                return;

            folderPath.Text = openDialog.FolderName;
        }
    }
}
