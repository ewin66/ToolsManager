using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Windows.Threading;
using ToolsManager.DataServices.Client;
using asyncTasks = System.Threading.Tasks;

namespace ToolsManager.Client.Observers
{
    public static class ScheduleObserver
    {
        private static DispatcherTimer _timer;
        private static EntityManager _entities;

        public static void Start()
        {
            _entities = EntityManager.Create();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(300); //5 min.
            _timer.Tick += new EventHandler(OnTimerElapsed);

            _timer.Start();
        }

        private static void OnTimerElapsed(object sender, EventArgs e)
        {
            foreach (ITaskRepository repository in _entities)
            {
                foreach (TaskInfo task in repository.Items)
                {
                    if (task.Schedule == null || !task.Schedule.IsEnabled)
                        continue;

                    if (IsTimeOver(task.Schedule.Time, task.LastExecution))
                        ExecuteTask(task);
                }
            }
        }

        private static bool IsTimeOver(DateTime scheduledTime, DateTime lastExecution)
        {
            DateTime nextExecution = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, scheduledTime.Hour, scheduledTime.Minute, scheduledTime.Second);

            if (DateTime.Now.CompareTo(nextExecution) == -1)
                return false;

            if (lastExecution == DateTime.MinValue)
                return true;

            if (lastExecution.CompareTo(nextExecution) == -1)
                return true;

            return false;
        }

        private static void ExecuteTask(TaskInfo taskInfo)
        {
            var taskManager = TasksManager.Create();

            ITool taskEngine = taskManager.CreateTaskEngineInstance(taskInfo.Category);

            taskInfo.Status = TaskStatus.Running;
            taskInfo.LastExecution = DateTime.Now;

            taskEngine.SetParameters(taskInfo.Parameters);
            var taskAsync = taskEngine.RunAsync();

            taskAsync.ContinueWith((t) =>
            {
                IMessages messages = (IMessages)t.Result;
                Logger.AddMessages(messages);
                taskInfo.Status = messages.HasErrors ? TaskStatus.Error : TaskStatus.Success;
            }, asyncTasks.TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
