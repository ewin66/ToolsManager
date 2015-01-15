using Atlante.Common;

namespace ToolsManager.Client.ViewModels
{
    public class TaskScheduleViewModel
    {
        public TaskSchedule Schedule { get; private set; }

        public TaskScheduleViewModel(TaskSchedule schedule)
        {
            this.Schedule = new TaskSchedule();

            if (schedule != null)
            {
                this.Schedule.Time = schedule.Time;
                this.Schedule.IsEnabled = schedule.IsEnabled;
            }
        }
    }
}
