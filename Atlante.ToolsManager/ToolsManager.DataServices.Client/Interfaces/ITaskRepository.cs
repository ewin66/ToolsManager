using Atlante.Common;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ToolsManager.DataServices.Client
{
    public interface ITaskRepository
    {
        List<TaskInfo> Items { get; }

        string Description { get; }

        bool Modified { get; }

        NotifyCollectionChangedEventHandler CollectionChanged { get; set; }

        //CRUD operations
        void Create(TaskInfo entity);
        void Update(TaskInfo entity);
        void Delete(TaskInfo entity);

        void SaveChanges();
        void CancelChanges();
    }
}
