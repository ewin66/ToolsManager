using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ToolsManager.Client.Views;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client;
using asyncTasks = System.Threading.Tasks;

namespace ToolsManager.Client.ViewModels
{
    public class TaskFlowViewModel : TaskViewModel
    {
        private TaskInfo selectedSuccessBranchTask;
        private TaskInfo selectedErrorBranchTask;

        public Command SelectFlowTaskCommand { get; private set; }
        public Command SelectionTypeCommand { get; private set; }

        public TaskInfo SelectedSuccessBranchTask
        {
            get { return selectedSuccessBranchTask; }
            set
            {
                selectedSuccessBranchTask = value;

                base.NotifyPropertyChanged("SelectedSuccessBranchTask");
            }
        }

        public TaskInfo SelectedErrorBranchTask
        {
            get { return selectedErrorBranchTask; }
            set
            {
                selectedErrorBranchTask = value;

                base.NotifyPropertyChanged("SelectedErrorBranchTask");
            }
        }

        public TaskFlowViewModel(ViewTemplate template, ITaskRepository repository)
            : base(template, repository)
        {
            this.SelectFlowTaskCommand = new Command("SelectTask", "SelectTask", "", this.ExecuteSelectFlowTask);
        }

        public void ExecuteSelectFlowTask(object parameter)
        {
            base.SelectedTask = (parameter as TaskInfo);

            if (base.SelectedTask == null)
                this.SelectedSuccessBranchTask = null;
            else
                this.SelectedSuccessBranchTask = this.GetBranchTask(this.SelectedTask.SuccessBranchTaskId);

            if (base.SelectedTask == null)
                this.SelectedErrorBranchTask = null;
            else
                this.SelectedErrorBranchTask = this.GetBranchTask(this.SelectedTask.ErrorBranchTaskId);
        }

        protected override void ExecuteProcessTask(object parameter)
        {
            if (this.SelectedTask == null)
                return;

            var task = this.SelectedTask;

            ITool taskEngine = base._tasksManager.CreateTaskEngineInstance(task.Category);

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

                    if (lastSelectedTask.HasSuccessBranch && lastSelectedTask.Status == TaskStatus.Success)
                    {
                        this.SelectedTask = this.GetBranchTask(lastSelectedTask.SuccessBranchTaskId);
                        this.ExecuteProcessTask(parameter);
                    }
                }, asyncTasks.TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                IMessages messages = taskEngine.Run();
                Logger.AddMessages(messages);
                lastSelectedTask.Status = messages.HasErrors ? TaskStatus.Error : TaskStatus.Success;

                if (lastSelectedTask.HasErrorBranch && lastSelectedTask.Status == TaskStatus.Error)
                {
                    this.SelectedTask = this.GetBranchTask(lastSelectedTask.ErrorBranchTaskId);
                    this.ExecuteProcessTask(parameter);
                }
            }
        }

        protected override void ConfigureView()
        {
            this.CurrentView = Activator.CreateInstance<TaskFlowView>();
            this.CurrentView.DataContext = this;

            this.ConfigureBrnachs();
        }

        public void ChangeSelectedSuccessBranch(object task)
        {
            var taskInfo = (task as TaskInfo);
            if (taskInfo == null || base.SelectedTask == null)
                return;

            base.SelectedTask.SuccessBranchTaskId = taskInfo.Id;

            this.UpdateSuccessTargetBranch(base.SelectedTask);
        }

        public void ChangeSelectedErrorBranch(object task)
        {
            var taskInfo = (task as TaskInfo);
            if (taskInfo == null || base.SelectedTask == null)
                return;

            base.SelectedTask.ErrorBranchTaskId = taskInfo.Id;

            this.UpdateErrorTargetBranch(base.SelectedTask);
        }

        protected override void ConfigurePersonalizedCommands()
        {
            this.SelectionTypeCommand = new Command("UI_MULTIPLE_SELECTION", "UI_MULTIPLE_SELECTION", "multipleSelection.png", this.ExecuteAllowMultipleSelection);

            this.Commands.Add(this.SelectionTypeCommand);
            this.Commands.Add(new Command("UI_ALIGN_LEFT", "UI_ALIGN_LEFT", "alignLeft.png", this.ExecuteAlignLeft, CanExecuteAlignment));
            this.Commands.Add(new Command("UI_ALIGN_BOTTOM", "UI_ALIGN_BOTTOM", "alignBottom.png", this.ExecuteAlignBottom, CanExecuteAlignment));
        }

        private void ConfigureBrnachs()
        {
            foreach (var task in base.Tasks)
            {
                if (task.HasSuccessBranch)
                    this.UpdateSuccessTargetBranch(task);
                if (task.HasErrorBranch)
                    this.UpdateErrorTargetBranch(task);
            }
        }

        private void UpdateSuccessTargetBranch(TaskInfo task)
        {
            var targetTask = this.GetBranchTask(task.SuccessBranchTaskId);
            if (targetTask != null)
            {
                task.TargetSuccessPosX = targetTask.PosX - 5;
                task.TargetSuccessPosY = targetTask.PosY + 35;
            }
        }

        private void UpdateErrorTargetBranch(TaskInfo task)
        {
            var targetTask = this.GetBranchTask(task.ErrorBranchTaskId);
            if (targetTask != null)
            {
                task.TargetErrorPosX = targetTask.PosX - 5;
                task.TargetErrorPosY = targetTask.PosY + 45;
            }
        }

        private TaskInfo GetBranchTask(Guid taskId)
        {
            return base.Tasks.Where(t => t.Id == taskId).FirstOrDefault();
        }

        private void ChangeLeftPosition(IEnumerable<TaskInfo> tasks, double left)
        {
            foreach (var task in base.SelectedTasks)
                task.PosX = left;
        }

        private void ChangeBottomPosition(IEnumerable<TaskInfo> tasks, double bottom)
        {
            foreach (var task in base.SelectedTasks)
                task.PosY = bottom;
        }

        private bool CanExecuteAlignment(object parameter)
        {
            return base.SelectedTasks.Count() >= 2;
        }

        private void ExecuteAllowMultipleSelection(object parameter)
        {
            base.AllowMultipleSelection = !base.AllowMultipleSelection;

            foreach (var task in base.Tasks)
            {
                if (task.Id != base.SelectedTask.Id)
                    task.IsSelected = false;
            }

            if (base.AllowMultipleSelection)
                this.SelectionTypeCommand.UpdateCommand("UI_SINGLE_SELECTION", "UI_SINGLE_SELECTION", "singleSelection.png");
            else
                this.SelectionTypeCommand.UpdateCommand("UI_MULTIPLE_SELECTION", "UI_MULTIPLE_SELECTION", "multipleSelection.png");
        }

        private void ExecuteAlignLeft(object parameter)
        {
            double left = double.MaxValue;
            foreach (var task in base.SelectedTasks)
            {
                if (task.PosX < left)
                    left = task.PosX;
            }

            this.ChangeLeftPosition(base.SelectedTasks, left);
        }

        private void ExecuteAlignBottom(object parameter)
        {
            double bottom = 0;
            foreach (var task in base.SelectedTasks)
            {
                if (task.PosY > bottom)
                    bottom = task.PosY;
            }

            this.ChangeBottomPosition(base.SelectedTasks, bottom);
        }
    }
}
