using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using ToolsManager.Client.Forms;
using ToolsManager.Client.Views;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client;

namespace ToolsManager.Client.ViewModels
{
    public class SharedTaskViewModel : BaseViewModel
    {
        private EntityManager _entities;
        private ViewsManager _dataManager;
        private ViewsManager _viewsManager;

        public SharedTask SelectedSharedTask { get; set; }

        public override string Title
        {
            get { return Translator.Translate("UI_SHARED_TASKS"); }
        }

        public ObservableCollection<CommandBase> Commands { get; private set; }
        public ObservableCollection<SharedTask> SharedTasks { get; private set; }

        public SharedTaskViewModel()
        {
            _entities = EntityManager.Create();
            _dataManager = ViewsManager.Create();
            _viewsManager = ViewsManager.Create();

            this.LoadData();
            this.LoadCommands();
        }

        private void LoadData()
        {
            Logger.AddTrace("Loading Data");

            this.SharedTasks = new ObservableCollection<SharedTask>();

            if (_viewsManager.SharedTasks == null)
                return;

            foreach (SharedTask sharedTask in _viewsManager.SharedTasks.Items.OrderBy(s => s.ViewId))
                this.SharedTasks.Add(sharedTask);

            _viewsManager.SharedTasks.CollectionChanged += new NotifyCollectionChangedEventHandler(SharedTasksSourceCollectionChanged);
        }

        private void LoadCommands()
        {
            Logger.AddTrace("Loading Commands");

            this.Commands = new ObservableCollection<CommandBase>();

            this.Commands.Add(new Command("UI_ACCEPT", "UI_ACCEPT_TASK", "accept24x24.png", this.ExecuteAcceptTask, this.CanExecuteAcceptTask));
            this.Commands.Add(new Command("UI_REMOVE", "UI_REMOVE_TASK", "delete24x24.png", this.ExecuteRejectTask, this.CanExecuteRejectTask));
            this.Commands.Add(new CommandSeparator());
            this.Commands.Add(new Command("UI_REFRESH", "UI_REFRESH_LIST", "refresh24x24.png", this.ExecuteRefresh));
        }

        private void ExecuteRefresh(object parameter)
        {
            Logger.AddTrace("Excuting Refresh Shared Tasks");

            _dataManager.LoadSharedData();
        }

        private void ExecuteAcceptTask(object parameter)
        {
            Logger.AddTrace("Executing Accept Shared Task");

            var desiredViewTemplate = _dataManager.Views.Items.Where(v => v.Id == this.SelectedSharedTask.ViewId).FirstOrDefault();

            if (desiredViewTemplate == null)
                this.CreateViewTemplate(this.SelectedSharedTask.ViewId, this.SelectedSharedTask.ViewDescription);

            var repositoryTasks = _entities[this.SelectedSharedTask.ViewId];

            if (repositoryTasks == null)
            {
                Logger.AddWarning(string.Format("There is no valid repository '{0}/{1}' for the shared task '{2}'.", this.SelectedSharedTask.ViewId, this.SelectedSharedTask.ViewDescription, this.SelectedSharedTask.Task.Description));
                return;
            }

            var addedTask = repositoryTasks.Items.Where(s => s.Id == this.SelectedSharedTask.Task.Id).FirstOrDefault();

            if (addedTask == null)
                repositoryTasks.Create(this.SelectedSharedTask.Task);
            else
            {
                addedTask.Category = this.SelectedSharedTask.Task.Category;
                addedTask.Description = this.SelectedSharedTask.Task.Description;
                addedTask.Parameters = this.SelectedSharedTask.Task.Parameters;
            }

            //Download files if were shared
            this.DownloadFiles(this.SelectedSharedTask.MachineTarget, this.SelectedSharedTask.Task);

            //Delete shared task
            ViewsManager.UnshareTask(this.SelectedSharedTask.MachineSource, this.SelectedSharedTask.ViewId, this.SelectedSharedTask.Task);

            this.SharedTasks.Remove(this.SelectedSharedTask);
        }

        private void ExecuteRejectTask(object parameter)
        {
            Logger.AddTrace("Executing Reject Shared Task");

            //Delete shared task
            ViewsManager.UnshareTask(this.SelectedSharedTask.MachineSource, this.SelectedSharedTask.ViewId, this.SelectedSharedTask.Task);

            this.SharedTasks.Remove(this.SelectedSharedTask);
        }

        private bool CanExecuteAcceptTask(object parameter)
        {
            return (this.SelectedSharedTask != null);
        }

        private bool CanExecuteRejectTask(object parameter)
        {
            return (this.SelectedSharedTask != null);
        }

        private void SharedTasksSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                this.SharedTasks.Clear();

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var sharedTask = item as SharedTask;

                    if (sharedTask != null)
                        this.SharedTasks.Add(sharedTask);
                }
            }
        }

        private void CreateViewTemplate(Guid id, string description)
        {
            var viewTemplate = new ViewTemplate() { Id = id, Description = description };

            ViewTemplateEntryView view = new ViewTemplateEntryView();
            view.DataContext = viewTemplate;

            if (!base.ShowDialog(Translator.Translate("UI_CATEGORY"), view))
                return;

            _dataManager.Views.Add(viewTemplate);
            _dataManager.SaveChanges();
        }

        private void DownloadFiles(string machine, TaskInfo task)
        {
            Logger.AddTrace("Downloading Files");

            if (string.IsNullOrEmpty(task.GetSharedPath()))
                return;

            DownloadOptionsForm form = new DownloadOptionsForm();
            form.Owner = Application.Current.MainWindow;
            form.ShowDialog();

            DownloadAction action = form.SelectedAction;

            switch (action)
            {
                case DownloadAction.Overwrite:
                    this.OverwriteFiles(machine, task);
                    break;
                case DownloadAction.Merge:
                    this.MergeFiles(machine, task);
                    break;
                default:
                    break;
            }
        }

        private void OverwriteFiles(string machine, TaskInfo task)
        {
            Logger.AddTrace("Overwriting Files");

            string targetZipFile;
            string sharedPath = FileUtilities.GetAbsolutePath(task.GetSharedPath());
            string targetPath = Path.GetDirectoryName(sharedPath);

            if (!ViewsManager.DownloadTaskFile(machine, task, out targetZipFile, task.GetSharedPath()))
                return;

            try
            {
                ZipFiles.Decompress(targetPath, targetZipFile);
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }
        }

        private void MergeFiles(string machine, TaskInfo task)
        {
            Logger.AddTrace("Merging Shared Task");

            string targetZipFile;
            string sharedPath = FileUtilities.GetAbsolutePath(task.GetSharedPath());
            string targetPath = Path.Combine(Path.GetTempPath(), task.Id.ToString());

            if (!ViewsManager.DownloadTaskFile(machine, task, out targetZipFile, task.GetSharedPath()))
                return;

            //clean temporal path before decompress new files
            if (Directory.Exists(targetPath))
                Directory.Delete(targetPath, true);

            try
            {
                ZipFiles.Decompress(targetPath, targetZipFile);
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }

            bool isDirectory = string.IsNullOrEmpty(Path.GetExtension(sharedPath));
            if (!isDirectory)
                targetPath = Path.Combine(targetPath, Path.GetFileName(sharedPath));

            FileUtilities.ForceCreateDirectory(sharedPath);

            this.ExecuteMergeTool(sharedPath, targetPath, isDirectory);
        }

        private void ExecuteMergeTool(string sourcePath, string targetPath, bool recursive)
        {
            Logger.AddTrace("Executing Merge Tool");

            var tasksManager = TasksManager.Create();

            ITool taskEngine = tasksManager.CreateTaskEngineInstance("MergeFiles");

            if (taskEngine == null)
            {
                Logger.AddWarning("Can't find MergeFiles tool.");
                return;
            }

            var parametersDefinition = taskEngine.GetParametersDefinition();

            var parameters = new Parameters();
            foreach (var param in parametersDefinition)
            {
                switch (param.Key)
                {
                    case "Path1":
                        parameters.Add(new Parameter() { Key = "Path1", Value = sourcePath });
                        break;
                    case "Path2":
                        parameters.Add(new Parameter() { Key = "Path2", Value = targetPath });
                        break;
                    case "Recursive":
                        parameters.Add(new Parameter() { Key = "Recursive", Value = recursive.ToString() });
                        break;
                    case "ExcludeFileSpecs":
                        parameters.Add(new Parameter() { Key = "ExcludeFileSpecs", Value = string.Empty });
                        break;
                    default:
                        Logger.AddWarning(string.Format("The parameter {0} doesn't exists in the MergeFiles tool.", param.Key));
                        break;
                }
            }

            if (parameters.Items.Count != parametersDefinition.Count())
                return;

            taskEngine.SetParameters(parameters);
            taskEngine.Run();
        }
    }
}
