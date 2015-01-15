using Atlante.Common;
using Atlante.Mef.Interfaces;
using Atlante.Presentation.Controls;
using Atlante.Presentation.Interfaces;
using Atlante.Presentation.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ToolsManager.Client.Observers;
using ToolsManager.Client.Views;
using ToolsManager.DataServices.Client;

namespace ToolsManager.Client.ViewModels
{
    public class MainViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private UIElement _currentView;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command MachinesCommand { get; private set; }
        public Command ViewTemplatesCommand { get; private set; }
        public Command SettingsCommand { get; private set; }
		public Command VariablesCommand { get; private set; }
        public Command SelectAddInCommand { get; private set; }
        public Command AboutCommand { get; private set; }

        public Visibility MessagesVisibility { get; private set; }

        public ToolsViewModel ToolsVM { get; private set; }
        public ApplicationViewModel ApplicationVM { get; private set; }
        public AddInsViewModel AddInsVM { get; private set; }

        public ObservableCollection<ICollapsablePanel> CollapsablePanels { get; private set; }

        public UIElement CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;

                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentView"));
            }
        }

        public MainViewModel()
        {
            this.ConfigureViewModels();
            this.ConfigureCommands();

            this.LoadTheme();

            this.LoadCollapsablePanels();

            ScheduleObserver.Start();
            SharedTaskObserver.Start();
        }

        private void ConfigureViewModels()
        {
            Logger.AddTrace("Configuring View Models");

            this.ToolsVM = new ToolsViewModel();
            this.ApplicationVM = ApplicationViewModel.Create();
            this.AddInsVM = new AddInsViewModel();

            this.ToolsVM.OnSelectionChanged += new Action<TaskViewModel>(this.ChangeTaskView);
            this.ToolsVM.SelectedTaskViewModel = this.ToolsVM.TaskViewModels.FirstOrDefault();
        }

        public void ConfigureCommands()
        {
            Logger.AddTrace("Configuring Commands");

            this.MachinesCommand = new Command("UI_MACHINES", this.ExecuteOpenMachinesForm);
            this.ViewTemplatesCommand = new Command("UI_CATEGORIES", this.ExecuteOpenViewTemplatesForm);
            this.SettingsCommand = new Command("UI_SETTINGS", this.ExecuteOpenSettings);
			this.VariablesCommand = new Command("UI_VARIABLES", this.ExecuteOpenVariables);
            this.SelectAddInCommand = new Command(String.Empty, this.ExecuteChangeAddInView);
            this.AboutCommand = new Command("UI_ABOUT", this.ExecuteOpenAbout);
        }

        private void LoadTheme()
        {
            Logger.AddTrace("Loading Theme");

            ResourceDictionary resource = new ResourceDictionary();

            resource.Source = new Uri(string.Format(@"pack://application:,,,/Atlante.Presentation;component/Themes/{0}.xaml", Properties.Settings.Default.Theme), UriKind.Absolute);

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resource);
        }

        private void LoadCollapsablePanels()
        {
            Logger.AddTrace("Loading Collapsable Panels");

            this.CollapsablePanels = new ObservableCollection<ICollapsablePanel>();
            this.CollapsablePanels.Add(this.ToolsVM);
        }

        private void ExecuteOpenMachinesForm(object parameter)
        {
            MachinesManager dataManager = new MachinesManager();

            MachinesView view = new MachinesView();
            view.DataContext = new MachinesViewModel(dataManager.Machines);

            if (!base.ShowDialog(Translator.Translate("UI_MACHINES"), view))
                return;

            dataManager.SaveChanges();
        }

        private void ExecuteOpenViewTemplatesForm(object parameter)
        {
            ViewsManager dataManager = ViewsManager.Create();

            ViewTemplateView view = new ViewTemplateView();
            view.DataContext = new ViewTemplateViewModel(dataManager.Views);

            if (!base.ShowDialog(Translator.Translate("UI_CATEGORIES"), view))
                return;

            dataManager.SaveChanges();
        }

        private void ExecuteOpenSettings(object parameter)
        {
            var view = new PropertiesEditor();
            var viewModel = new SettingsViewModel();
            view.DataContext = viewModel;

            if (!base.ShowDialog(Translator.Translate("UI_SETTINGS"), view))
                return;

            viewModel.SaveChanges();
        }

		private void ExecuteOpenVariables(object parameter)
		{
			var view = new PropertiesEditor();
			var viewModel = new VariablesViewModel();
			view.DataContext = viewModel;

			if (!base.ShowDialog(Translator.Translate("UI_VARIABLES"), view))
				return;

			viewModel.SaveChanges();
		}

        private void ExecuteChangeAddInView(object parameter)
        {
            var selectedAddIn = parameter as IAddIn;

            this.CurrentView = selectedAddIn.CurrentView;
            this.ApplicationVM.SetAddInDescription(selectedAddIn.ApplicationDescription);
        }

        private void ExecuteOpenAbout(object parameter)
        {
            var view = new AboutView();
            view.DataContext = this.ApplicationVM;

            base.ShowDialog(Translator.Translate("UI_ABOUT"), view);
        }

        private void ChangeTaskView(TaskViewModel taskViewModel)
        {
            if (taskViewModel != null)
                this.CurrentView = taskViewModel.CurrentView;
            else
                this.CurrentView = null;

            this.ApplicationVM.SetAddInDescription(string.Empty);
        }
    }
}
