using Atlante.Common;
using Atlante.Mef.Interfaces;
using Atlante.Presentation;
using Atlante.Presentation.ViewModels;
using System;
using ToolsManager.Client.Views;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client;
using asyncTasks = System.Threading.Tasks;

namespace ToolsManager.Client.ViewModels
{
    public class TaskTableViewModel : TaskViewModel
    {
        private TaskInfo _copiedTask;

        public TaskTableViewModel(ViewTemplate template, ITaskRepository repository)
            : base(template, repository)
        {
        }

        protected override void ExecuteProcessTask(object parameter)
        {
            if (this.SelectedTasks == null)
                return;

            foreach (var task in this.SelectedTasks)
            {
                ITool taskEngine = base._tasksManager.CreateTaskEngineInstance(task.Category);

				task.ReplaceVariables(base.ToolsVariables);

                taskEngine.SetParameters(task.Parameters);
                taskEngine.SetScreen(this);

                task.Status = TaskStatus.Running;
                task.LastExecution = DateTime.Now;

                var lastSelectedTask = task;

                if (taskEngine.IsAsynchronous)
                {
                    var asyncTask = taskEngine.RunAsync();
                    asyncTask.ContinueWith((t) =>
                    {
                        IMessages messages = (IMessages)t.Result;
                        Logger.AddMessages(messages);
                        lastSelectedTask.Status = messages.HasErrors ? TaskStatus.Error : TaskStatus.Success;
                    }, asyncTasks.TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    IMessages messages = taskEngine.Run();
                    Logger.AddMessages(messages);
                    lastSelectedTask.Status = messages.HasErrors ? TaskStatus.Error : TaskStatus.Success;
                }
            }
        }

        protected override void ConfigurePersonalizedCommands()
        {
            base.Commands.Add(new Command("UI_COPY", "UI_COPY_TASK", "copy.png", this.ExecuteCopyTask, base.CanExecuteTaskCommand));
            base.Commands.Add(new Command("UI_PASTE", "UI_PASTE_TASK", "paste.png", this.ExecutePasteTask, this.CanExecutePastTask));
            base.Commands.Add(new Command("UI_ADD", "UI_ADD_TASK", "add24x24.png", this.ExecuteNewTask));
            base.Commands.Add(new Command("UI_REMOVE", "UI_REMOVE_TASK", "delete24x24.png", this.ExecuteRemoveTask, base.CanExecuteTaskCommand));
            base.Commands.Add(new Command("UI_SHARE", "UI_SHARE_TASK", "share24x24.png", this.ExecuteShareTask, base.CanExecuteTaskCommand));
        }

        protected override void ConfigureView()
        {
            this.CurrentView = Activator.CreateInstance<TaskView>();
            this.CurrentView.DataContext = this;
        }

        private bool CanExecutePastTask(object parameter)
        {
            return _copiedTask != null;
        }

        private void ExecuteCopyTask(object parameter)
        {
            _copiedTask = this.SelectedTask;
        }

        private void ExecutePasteTask(object parameter)
        {
            if (_copiedTask == null)
                return;

            TaskInfo task = new TaskInfo();
            task.Id = Guid.NewGuid();
            task.Description = string.Format("{0} - copy", _copiedTask.Description);
            task.Category = _copiedTask.Category;

            foreach (var param in _copiedTask.Parameters.Items)
                task.Parameters.Add(new Parameter() { Key = param.Key, Value = param.Value, IsSystem = param.IsSystem });

            this.Tasks.Add(task);

            _copiedTask = null;
        }

        private void ExecuteNewTask(object parameter)
        {
            var newTask = new TaskInfo() { Id = Guid.NewGuid() };

            this.Tasks.Add(newTask);
        }

        private void ExecuteRemoveTask(object parameter)
        {
            if (this.SelectedTask == null)
                return;

            this.Tasks.Remove(this.SelectedTask);
        }

        private void ExecuteShareTask(object parameter)
        {
            if (this.SelectedTask == null)
                return;

            MachinesManager dataManager = new MachinesManager();

            MachinesView view = new MachinesView();
            MachinesViewModel machinesVM = new MachinesViewModel(dataManager.Machines, true);
            view.DataContext = machinesVM;

            if (!base.ShowDialog(Translator.Translate("UI_MACHINES"), view))
                return;

            base.ShareSelectedTasks(machinesVM.SelectedMachine.Name);
        }
    }
}
