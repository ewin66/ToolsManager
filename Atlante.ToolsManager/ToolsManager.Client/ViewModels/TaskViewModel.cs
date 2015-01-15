using Atlante.Common;
using Atlante.Mef.Interfaces;
using Atlante.Presentation;
using Atlante.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using ToolsManager.Client.Views;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client;
using asyncTasks = System.Threading.Tasks;

namespace ToolsManager.Client.ViewModels
{
    public class TaskViewModel : BaseViewModel, INotifyPropertyChanged
    {
        protected TasksManager _tasksManager;
        private ViewsManager _viewsManager;
        private TaskInfo _selectedTask;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool AllowMultipleSelection { get; set; }

        public ViewTemplate ViewTemplate { get; private set; }
        public ITaskRepository Repository { get; private set; }

        public UserControl CurrentView { get; set; }

        public Command OpenParametersCommand { get; private set; }

        public event Action<ViewTemplate> OnViewModeChanged;

        public TaskInfo SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                if (_selectedTask != null && !this.AllowMultipleSelection)
                    _selectedTask.IsSelected = false;

                _selectedTask = value;

                if (_selectedTask != null)
                    _selectedTask.IsSelected = true;
            }
        }

        public IEnumerable<TaskInfo> SelectedTasks
        {
            get { return this.Tasks.Where(t => t.IsSelected == true).AsEnumerable(); }
        }

		public ToolsVariables ToolsVariables
		{
			get { return _viewsManager.ToolsVariables; }
		}

        public ObservableCollection<TaskInfo> Tasks { get; protected set; }
        public ObservableCollection<string> EnabledTools { get; set; }
        public ObservableCollection<CommandBase> Commands { get; set; }

        public TaskViewModel(ViewTemplate template, ITaskRepository repository)
        {
            _viewsManager = ViewsManager.Create();
            _tasksManager = TasksManager.Create();

            this.Repository = repository;
            this.ViewTemplate = template;

            this.LoadData();
            this.ConfigureCommands();
            this.ConfigureView();
        }

        private void LoadData()
        {
            this.LoadTasks();
            this.LoadEnabledTools();
        }

        private void LoadTasks()
        {
            this.Tasks = new ObservableCollection<TaskInfo>();

            foreach (TaskInfo task in this.Repository.Items.OrderBy(t => t.Category).ThenBy(t => t.Description))
                this.Tasks.Add(task);

            this.Tasks.CollectionChanged += new NotifyCollectionChangedEventHandler(TasksViewCollectionChanged);
            this.Repository.CollectionChanged += new NotifyCollectionChangedEventHandler(TasksSourceCollectionChanged);
        }

        private void LoadEnabledTools()
        {
            this.EnabledTools = new ObservableCollection<string>();

            if (this.ViewTemplate.EnabledTools.Count > 0)
            {
                foreach (var category in this.ViewTemplate.EnabledTools)
                    this.EnabledTools.Add(category);
            }
            else
            {
                foreach (var category in _tasksManager.AvailableTools)
                    this.EnabledTools.Add(category);
            }
        }

        protected virtual void ConfigureView()
        {
            // Override the View used for each kind of Task Category
        }

        protected virtual void ConfigurePersonalizedCommands()
        {
            // Override the View used for each kind of Task Category
        }

        private void ConfigureCommands()
        {
            this.Commands = new ObservableCollection<CommandBase>();

            this.OpenParametersCommand = new Command("UI_PARAMETERS", "UI_SHOW_PARAMETERS", "editTable24x24.png", this.ExecuteOpenParametersDialog, this.CanExecuteTaskCommand);

            this.Commands.Add(new Command("UI_EXECUTE", "UI_EXECUTE_TASK", "execute24x24.png", this.ExecuteProcessTask, this.CanExecuteTaskCommand));
            this.Commands.Add(this.OpenParametersCommand);
            this.Commands.Add(new Command("UI_SCHEDULE", "UI_SCHEDULE_TASK", "schedule24x24.png", this.ExecuteScheduleTask, this.CanExecuteTaskCommand));
            this.Commands.Add(new CommandSeparator());

            this.ConfigurePersonalizedCommands();

            this.Commands.Add(new CommandSeparator());
            this.Commands.Add(new Command("UI_SAVE", "UI_SAVE_CHANGES", "save24x24.png", this.ExecuteSaveChanges, this.CanExecuteSaveChannges));
            this.Commands.Add(new CommandSeparator());
            this.Commands.Add(new Command("UI_VIEW_DETAILS", "UI_VIEW_DETAILS", "viewDetails.png", this.ExecuteShowViewDetails, this.CanExecuteShowViewDetailsCommand));
            this.Commands.Add(new Command("UI_VIEW_FLOW", "UI_VIEW_FLOW", "viewFlow.png", this.ExecuteShowViewFlow, this.CanExecuteShowViewFlowCommand));
        }

        protected bool CanExecuteTaskCommand(object parameter)
        {
            return (this.SelectedTask != null);
        }

        private bool CanExecuteSaveChannges(object parameter)
        {
            return this.Repository.Modified;
        }

        private bool CanExecuteShowViewDetailsCommand(object parameter)
        {
            return this.ViewTemplate.Type == "Flow";
        }

        private bool CanExecuteShowViewFlowCommand(object parameter)
        {
            return (this.ViewTemplate.Type == null || this.ViewTemplate.Type == "Table");
        }

        private void ExecuteShowViewDetails(object parameter)
        {
            this.ViewTemplate.Type = "Table";

            if (OnViewModeChanged != null)
                OnViewModeChanged(this.ViewTemplate);
        }

        private void ExecuteShowViewFlow(object parameter)
        {
            this.ViewTemplate.Type = "Flow";

            if (OnViewModeChanged != null)
                OnViewModeChanged(this.ViewTemplate);
        }

        private void ExecuteOpenParametersDialog(object parameter)
        {
            if (this.SelectedTask == null)
                return;

            ITool taskEngine = _tasksManager.CreateTaskEngineInstance(this.SelectedTask.Category);
            taskEngine.ConfigureParameters(this.SelectedTask);

            ParametersView view = new ParametersView();
            view.DataContext = new ParametersViewModel(this.SelectedTask.Parameters);

            if (!base.ShowDialog(Translator.Translate("UI_PARAMETERS"), view))
                return;
        }

        protected virtual void ExecuteProcessTask(object parameter)
        {
            // Override based on the type table/flow
        }

        private void ExecuteScheduleTask(object parameter)
        {
            if (this.SelectedTask == null)
                return;

            var viewModel = new TaskScheduleViewModel(this.SelectedTask.Schedule);
            var view = new TaskScheduleView();
            view.DataContext = viewModel;

            if (!base.ShowDialog(Translator.Translate("UI_TASK_SCHEDULE"), view))
                return;

            this.SelectedTask.Schedule = viewModel.Schedule;
        }
        
        protected async void ShareSelectedTasks(string machineName)
        {
            ApplicationViewModel.BeginAction(ActionType.ShareTask);
            try
            {
                foreach (var task in this.SelectedTasks)
                    await _viewsManager.ShareTaskAsync(machineName, this.ViewTemplate, task, task.GetSharedPath());
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }
            finally
            {
                ApplicationViewModel.EndAction(ActionType.ShareTask);
            }
        }

        private void ExecuteSaveChanges(object parameter)
        {
            this.Repository.SaveChanges();
        }

        private void ExecuteCancelChanges(object parameter)
        {
            this.Repository.CancelChanges();
        }

        public void ExecuteDeactivate()
        {
            if (!this.Repository.Modified)
                return;

            //if ( MessageBox.Show( "Do you want to save changes?", "Confirmation", MessageBoxButton.YesNo ) == MessageBoxResult.No )
            //{
            //    this.ExecuteCancelChanges( null );
            //    return;
            //}

            this.ExecuteSaveChanges(null);
        }

        private void TasksViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var task = e.NewItems[0] as TaskInfo;

                if (task != null && !this.Repository.Items.Contains(task))
                    this.Repository.Create(task);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var task = e.OldItems[0] as TaskInfo;

                if (task != null)
                    this.Repository.Delete(task);
            }
        }

        private void TasksSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var task = e.NewItems[0] as TaskInfo;

                if (task != null && !this.Tasks.Contains(task))
                    this.Tasks.Add(task);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var task = e.OldItems[0] as TaskInfo;

                if (task != null)
                    this.Tasks.Remove(task);
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }
}
