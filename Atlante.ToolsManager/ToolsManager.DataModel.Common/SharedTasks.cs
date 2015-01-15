using Atlante.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace ToolsManager.DataModel.Common
{
    [Serializable]
    public class SharedTask
    {
        public string MachineSource { get; set; }
        public string MachineTarget { get; set; }

        public Guid ViewId { get; set; }
        public string ViewDescription { get; set; }

        public TaskInfo Task { get; set; }

        public SharedTask()
        {
            //
        }
    }

    [Serializable]
    public class SharedTasks
    {
        public List<SharedTask> Items { get; set; }

        public SharedTask this[int index]
        {
            get
            {
                return this.Items[index];
            }
        }

        [XmlIgnore()]
        public NotifyCollectionChangedEventHandler CollectionChanged { get; set; }

        public SharedTasks()
        {
            this.Items = new List<SharedTask>();
        }

        public void Add(SharedTask item)
        {
            this.Items.Add(item);

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void AddRange(SharedTask[] items)
        {
            foreach (var item in items)
                this.Items.Add(item);

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void Clear()
        {
            this.Items.Clear();

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null));
        }

        public void Remove(SharedTask item)
        {
            this.Items.Remove(item);
        }
    }
}
