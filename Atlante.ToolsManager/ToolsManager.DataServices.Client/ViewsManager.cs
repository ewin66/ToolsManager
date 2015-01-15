using Atlante.Common;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using ToolsManager.DataModel.Common;
using ToolsManager.DataServices.Client.FileServiceProxy;
using ToolsManager.DataServices.Client.ShareServiceProxy;
using ToolsManager.DataServices.Common;

namespace ToolsManager.DataServices.Client
{
    public class ViewsManager
    {
        private string _templatePath;
		private string _variablesPath;

        private static ViewsManager _manager;

        public ViewTemplates Views { get; private set; }
        public SharedTasks SharedTasks { get; private set; }
		public ToolsVariables ToolsVariables { get; private set; }

        private ViewsManager()
        {
            _templatePath = ConfigurationManager.AppSettings["viewTemplates"];
			_variablesPath = ConfigurationManager.AppSettings["toolsVariables"];

            if (_templatePath == null)
                Logger.AddWarning("There is no configuration value for 'viewTemplates'");

			if (_variablesPath == null)
				Logger.AddWarning("There is no configuration value for 'toolsVariables'");

            this.LoadData();

            this.SharedTasks = new SharedTasks();
        }

        public static ViewsManager Create()
        {
            Logger.AddTrace("Creating Views Manager");

            if (_manager == null)
                _manager = new ViewsManager();

            return _manager;
        }

        private void LoadData()
        {
            Logger.AddTrace("Loading Data");

            this.Views = Serializer.Deserialize<ViewTemplates>(_templatePath);
			this.ToolsVariables = Serializer.Deserialize<ToolsVariables>(_variablesPath);

            if (this.Views == null)
				this.Views = new ViewTemplates();

			if (this.ToolsVariables == null)
				this.ToolsVariables = new ToolsVariables();
        }

        public void LoadSharedData()
        {
            Logger.AddTrace("Loading Shared Data");

            this.SharedTasks.Clear();

            try
            {
                ShareManagerServiceClient shareServices = new ShareManagerServiceClient();

                SharedTaskDTO[] sharedTasks = shareServices.GetSharedTasks(Environment.MachineName);

                foreach (SharedTaskDTO task in sharedTasks)
                {
                    ObjectMapper mapper = new ObjectMapper(typeof(SharedTask), typeof(SharedTaskDTO));
                    SharedTask sharedTask = mapper.Map(task, true) as SharedTask;

                    this.SharedTasks.Add(sharedTask);
                }

                shareServices.Close();
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }
        }

        public async Task ShareTaskAsync(string machineTarget, ViewTemplate viewTemplate, TaskInfo task, string sharedPath)
        {
            Logger.AddTrace("Sharing Task");

            ShareManagerServiceClient shareServices = new ShareManagerServiceClient();

            ObjectMapper mapper = new ObjectMapper(typeof(TaskInfo), typeof(TaskInfoDTO));
            TaskInfoDTO taskDTO = mapper.Map(task, false) as TaskInfoDTO;

            shareServices.ShareTask(Environment.MachineName, machineTarget, viewTemplate.Id, viewTemplate.Description, taskDTO);

            shareServices.Close();

            await this.UploadTaskFileAsync(machineTarget, task, sharedPath);
        }

        public static void UnshareTask(string machineSource, Guid viewTemplateId, TaskInfo task)
        {
            Logger.AddTrace("Unsharing Task");

            ShareManagerServiceClient shareServices = new ShareManagerServiceClient();

            ObjectMapper mapper = new ObjectMapper(typeof(TaskInfo), typeof(TaskInfoDTO));
            var taskDTO = mapper.Map(task, false) as TaskInfoDTO;

            shareServices.UnshareTask(machineSource, Environment.MachineName, viewTemplateId, taskDTO);
        }

        private async Task UploadTaskFileAsync(string machineTarget, TaskInfo task, string sharedPath)
        {
            await Task.Factory.StartNew(() => UploadTaskFile(machineTarget, task, sharedPath));
        }

        private void UploadTaskFile(string machineTarget, TaskInfo task, string sharedPath)
        {
            if (string.IsNullOrEmpty(sharedPath))
                return;

            FileUploadServiceClient fileServices = new FileUploadServiceClient();

            var fileName = string.Format("{0}.{1}.{2}", machineTarget, task.Id.ToString(), ZipFiles.FILE_EXTENSION);

            var stream = ZipFiles.Compress(sharedPath);

            if (stream == null)
                return;

            fileServices.UploadFile(fileName, stream);
        }

        public static bool DownloadTaskFile(string machineTarget, TaskInfo task, out string targetZipFile, string sharedPath)
        {
            Logger.AddTrace("Downloading Shared Task");

            targetZipFile = string.Empty;

            if (string.IsNullOrEmpty(sharedPath))
                return false;

            FileUploadServiceClient fileServices = new FileUploadServiceClient();

            string fileName = string.Format("{0}.{1}.{2}", machineTarget, task.Id.ToString(), ZipFiles.FILE_EXTENSION);

            if (!fileServices.FileExists(fileName))
            {
                Logger.AddWarning(string.Format("The file {0} can't be downloaded from server", fileName));
                return false;
            }

            Stream stream = null;

            try
            {
                stream = fileServices.DownloadFile(fileName);

                targetZipFile = Path.Combine(Path.GetTempPath(), fileName);

                FileUtilities.SaveFile(stream, targetZipFile);

                fileServices.DeleteFile(fileName);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();

                fileServices.Close();
            }
        }

        public void SaveChanges()
        {
            Logger.AddTrace("Save Template Changes");

            Serializer.Serialize<ViewTemplates>(this.Views, _templatePath);
        }

        public bool HasPendingSharedTask()
        {
            bool arePendingSharedTaks = false;
            //try
            //{
            //    ShareManagerServiceClient shareServices = new ShareManagerServiceClient();
            //    int sharedTasksCount = shareServices.GetSharedTasksCount(Environment.MachineName);

            //    shareServices.Close();

            //    arePendingSharedTaks = sharedTasksCount > this.SharedTasks.Items.Count;

            //    if (arePendingSharedTaks)
            //        this.LoadSharedData();
            //}
            //catch (Exception e)
            //{
            //    Logger.AddException(e);
            //}

            return arePendingSharedTaks;
        }
    }
}
