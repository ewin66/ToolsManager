using Atlante.Common;
using Atlante.Presentation.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ToolsManager.Client.Views;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client;

namespace ToolsManager.Client.ViewModels
{
    public class ToolsViewModel : BaseViewModel, ICollapsablePanel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _visible = true;

        private ViewsManager _dataManager;
        private EntityManager _entities;
        private ViewsManager _viewsManager;
        private TaskViewModel _selectedTask;

        public event Action<TaskViewModel> OnSelectionChanged;

        public Command PinCommand { get; private set; }

        public ObservableCollection<Command> Commands { get; set; }
        public ObservableCollection<TaskViewModel> TaskViewModels { get; private set; }

        public string Header
        {
            get { return "Categories"; }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;

                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
            }
        }

        public TaskViewModel SelectedTaskViewModel
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;

                if (this.OnSelectionChanged != null)
                    this.OnSelectionChanged(value);
            }
        }

        public ToolsViewModel()
        {
            this.TaskViewModels = new ObservableCollection<TaskViewModel>();

            _dataManager = ViewsManager.Create();
            _entities = EntityManager.Create();
            _viewsManager = ViewsManager.Create();

            this.LoadData();
            this.ConfigureCommands();
            this.ConfigureHandlers();
        }

        private void LoadData()
        {
            Logger.AddTrace("Loading Data");

            this.TaskViewModels.Clear();

            foreach (ViewTemplate template in _dataManager.Views.Items.OrderBy(i => i.Description))
                this.AddTaskViewModel(template, this.TaskViewModels.Count);
        }

        private void ConfigureHandlers()
        {
            _dataManager.Views.CollectionChanged += new NotifyCollectionChangedEventHandler(ViewTemplatesSourceCollectionChanged);
        }

        private void ConfigureCommands()
        {
            Logger.AddTrace("Configuring Commands");

            this.Commands = new ObservableCollection<Command>();

            Commands.Add(new Command("Add", "Add view template", "group_add.png", this.ExecuteAddViewTemplate));
            Commands.Add(new Command("Modify", "Modify view template", "group_edit.png", this.ExecuteModifyViewTemplate, this.CanExecuteModifyViewTemplate));
            Commands.Add(new Command("Delete", "Delete view template", "group_delete.png", this.ExecuteDeleteViewTemplate, this.CanExecuteDeleteViewTemplate));
            Commands.Add(new Command("Share", "Share view template", "share24x24.png", this.ExecuteShareViewTemplate, this.CanExecuteShareViewTemplate));

            this.PinCommand = new Command("Pin", this.ExecuteChangeVisibility);
        }

        private bool CanExecuteModifyViewTemplate(object parameter)
        {
            return (this.SelectedTaskViewModel != null);
        }

        private bool CanExecuteDeleteViewTemplate(object parameter)
        {
            return (this.SelectedTaskViewModel != null);
        }

        private bool CanExecuteShareViewTemplate(object parameter)
        {
            return (this.SelectedTaskViewModel != null);
        }

        private void ExecuteAddViewTemplate(object parameter)
        {
            ViewTemplate viewTemplate = new ViewTemplate();
            viewTemplate.Id = Guid.NewGuid();

            ViewTemplateEntryView view = new ViewTemplateEntryView();
            view.DataContext = viewTemplate;

            if (!base.ShowDialog(Translator.Translate("UI_CATEGORY"), view))
                return;

            _dataManager.Views.Add(viewTemplate);
            _dataManager.SaveChanges();
        }

        private void ExecuteModifyViewTemplate(object parameter)
        {
            ViewTemplateEntryView view = new ViewTemplateEntryView();
            view.DataContext = this.SelectedTaskViewModel.ViewTemplate;

            if (!base.ShowDialog(Translator.Translate("UI_CATEGORY"), view))
                return;

            _dataManager.SaveChanges();
        }

        private void ExecuteDeleteViewTemplate(object parameter)
        {
            if (MessageBox.Show("Do you want to delete the view template?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            FileUtilities.ForceDeleteFile(this.SelectedTaskViewModel.ViewTemplate.ConfigFile);

            _dataManager.Views.Remove(this.SelectedTaskViewModel.ViewTemplate);
            _dataManager.SaveChanges();
        }

        public void ExecuteSaveViewTemplate(object parameter)
        {
            var viewTemplate = parameter as TaskViewModel;
            if (viewTemplate == null)
                return;

            viewTemplate.ExecuteDeactivate();
        }

        private void ExecuteShareViewTemplate(object parameter)
        {
            //to do: Make it singleton
            MachinesManager dataManager = new MachinesManager();

            if (this.SelectedTaskViewModel == null)
                return;

            MachinesView view = new MachinesView();
            MachinesViewModel machinesVM = new MachinesViewModel(dataManager.Machines, true);
            view.DataContext = machinesVM;

            if (!base.ShowDialog(Translator.Translate("UI_MACHINES"), view))
                return;

            foreach (TaskInfo task in this.SelectedTaskViewModel.Tasks)
                _viewsManager.ShareTaskAsync(machinesVM.SelectedMachine.Name, this.SelectedTaskViewModel.ViewTemplate, task, task.GetSharedPath());
        }

        private void ExecuteChangeVisibility(object parameter)
        {
            this.Visible = !this.Visible;
        }

        private void ViewTemplatesSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var viewTemplate = e.NewItems[0] as ViewTemplate;

                if (viewTemplate != null)
                    this.AddTaskViewModel(viewTemplate, this.TaskViewModels.Count);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var viewTemplate = e.OldItems[0] as ViewTemplate;

                if (viewTemplate != null)
                {
                    var taskViewModel = this.TaskViewModels.Where(t => t.ViewTemplate.Id == viewTemplate.Id).FirstOrDefault();

                    if (taskViewModel != null)
                        this.TaskViewModels.Remove(taskViewModel);
                }
            }
        }

        private void AddTaskViewModel(ViewTemplate viewTemplate, int index)
        {
            TaskViewModel taskViewModel = this.CreateTaskViewModel(viewTemplate);

            taskViewModel.OnViewModeChanged += ViewModeChanged;

            this.TaskViewModels.Insert(index, taskViewModel);
        }

        private TaskViewModel CreateTaskViewModel(ViewTemplate viewTemplate)
        {
            if (viewTemplate.Type == "Flow")
                return new TaskFlowViewModel(viewTemplate, _entities[viewTemplate.Id]);

            return new TaskTableViewModel(viewTemplate, _entities[viewTemplate.Id]);
        }

        private void ViewModeChanged(ViewTemplate viewTemplate)
        {
            var taskViewModel = this.TaskViewModels.Where(x => x.ViewTemplate.Id == viewTemplate.Id).FirstOrDefault();
            if (taskViewModel == null)
                return;

            var index = this.TaskViewModels.IndexOf(taskViewModel);
            this.TaskViewModels.Remove(taskViewModel);
            
            this.AddTaskViewModel(viewTemplate, index);

            this.SelectedTaskViewModel = this.TaskViewModels.Where(x => x.ViewTemplate.Id == viewTemplate.Id).FirstOrDefault();
        }
    }
}
