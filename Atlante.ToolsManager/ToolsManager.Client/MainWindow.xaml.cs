using Atlante.Common;
using System;
using System.Windows;
using System.Windows.Threading;
using ToolsManager.Client.ViewModels;

namespace ToolsManager.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.ConfigureLanguage();

            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        private void ConfigureLanguage()
        {
            Logger.AddTrace("Configuring Language");

            Translator.Language = Properties.Settings.Default.Language;
        }

        private void ApplicationInfoChanged()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                if (AppInfo.PendingSharedTasks && this.WindowState == WindowState.Minimized
                    || AppInfo.PendingSharedTasks && !this.IsActive)
                    this.FlashWindow();
            }));
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.StopFlashingWindow();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.StopFlashingWindow();
        }
    }
}
