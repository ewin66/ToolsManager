using System;
using System.Collections;
using System.Collections.Generic;
using Atlante.Common;
using ToolsManager.DataModel.Common;

namespace ToolsManager.DataServices.Client
{
    public class EntityManager : IEnumerable
    {
        private static EntityManager _manager;

        private List<ITaskRepository> _repositories;

        public ITaskRepository this[Guid id]
        {
            get
            {
                return this.RepositoryAt(id);
            }
        }

        public static EntityManager Create()
        {
            Logger.AddTrace("Creating Entity Manager");

            if (_manager == null)
                _manager = new EntityManager();

            return _manager;
        }

        private EntityManager()
        {
            _repositories = new List<ITaskRepository>();

            ViewsManager templatesManager = ViewsManager.Create();

            foreach (ViewTemplate template in templatesManager.Views.Items)
                this.CreateRepository(template);
        }

        private TaskRepository LoadData(string file)
        {
            var repository = Serializer.Deserialize<TaskRepository>(file);

            if (repository == null)
                repository = new TaskRepository();

            repository.Initialize();

            return repository;
        }

        public ITaskRepository CreateRepository(ViewTemplate template)
        {
            var repository = this.LoadData(template.ConfigFile);

            repository.Id = template.Id;
            repository.Description = template.Description;
            repository.File = template.ConfigFile;

            _repositories.Add(repository);

            return repository;
        }

        private ITaskRepository RepositoryAt(Guid id)
        {
            foreach (TaskRepository repository in _repositories)
                if (repository.Id == id)
                    return repository;

            var template = ViewsManager.Create().Views[id];
            return this.CreateRepository(template);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (ITaskRepository repository in _repositories)
                yield return repository;
        }
    }
}
