using System.ComponentModel;
using Atlante.Common;

namespace ToolsManager.Client.ViewModels
{
    public class BottomPanelViewModel : INotifyPropertyChanged
    {
        public string CurrentTitle
        {
            get
            {
                if ( CurrentModel == null )
                    return string.Empty;

                return CurrentModel.Title;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Command CloseCommand { get; set; }
        public Command SelectedViewCommand { get; set; }

        public BaseViewModel CurrentModel { get; set; }

        public bool HasCurrent { get { return CurrentModel != null; } }

        public BottomPanelViewModel( )
        {
            ConfigureCommands( );
        }

        private void ConfigureCommands( )
        {
            CloseCommand = new Command( "Close", ExecuteClose );
            SelectedViewCommand = new Command( "SelectedView", ExecuteSelectedView );
        }

        private void ExecuteClose( object parameter )
        {
            CurrentModel = null;

            PropertyChanged( this, new PropertyChangedEventArgs( "HasCurrent" ) );
        }

        private void ExecuteSelectedView( object parameter )
        {
            CurrentModel = parameter as BaseViewModel;

            PropertyChanged( this, new PropertyChangedEventArgs( "HasCurrent" ) );
            PropertyChanged( this, new PropertyChangedEventArgs( "CurrentTitle" ) );
        }
    }
}
