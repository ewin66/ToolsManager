using Atlante.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ToolsManager.DataServices.Client
{
    [Serializable]
    [XmlRoot("RepositoryOfTaskInfo")]
    public class TaskRepository : ITaskRepository
    {
        private List<TaskInfo> _items;

        [XmlArrayItem("TaskInfo")]
        public List<TaskInfo> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public Guid Id { get; set; }

        public string Description { get; set; }

        public string File { get; set; }

        [XmlIgnore()]
        public bool Modified { get; private set; }

        [XmlIgnore()]
        public NotifyCollectionChangedEventHandler CollectionChanged { get; set; }

        public TaskRepository()
        {
            _items = new List<TaskInfo>();
        }

        public void Initialize()
        {
            foreach (TaskInfo task in _items)
                this.ConfigureTaskPropertyChangedEvents(task);
        }

        public void Create(TaskInfo item)
        {
            _items.Add(item);

            this.Modified = true;

            this.ConfigureTaskPropertyChangedEvents(item);

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Update(TaskInfo item)
        {
            throw new NotImplementedException();
        }

        public void Delete(TaskInfo item)
        {
            _items.Remove(item);

            this.Modified = true;

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        public void SaveChanges()
        {
            Serializer.Serialize<TaskRepository>(this, this.File);

            this.Modified = false;
        }

        public void CancelChanges()
        {
            //todo

            this.Modified = false;
        }

        private void ConfigureTaskPropertyChangedEvents(TaskInfo task)
        {
            task.PropertyChanged += new PropertyChangedEventHandler(TaskPropertyChanged);
            task.Parameters.PropertyChanged += new PropertyChangedEventHandler(TaskPropertyChanged);
            foreach (Parameter param in task.Parameters.Items)
                param.PropertyChanged += new PropertyChangedEventHandler(TaskPropertyChanged);
        }

        private void TaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Modified = true;
        }
    }
}
