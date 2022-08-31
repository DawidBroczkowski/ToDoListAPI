using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class JsonTaskManager : ITaskManager
    {
        private Models.User _user;

        public JsonTaskManager(Models.User user)
        {
            _user = user;
        }

        public void CreateNewTask(string name, string? description)
        {
            Models.Task task = new()
            {
                Name = name,
                Description = description,
                Status = Status.New
            };
            _user.TodoList.Add(task);
        }

        public bool TryStartTask(Guid id)
        {
            return TryUpdateTaskStatus(id, Status.Current);
        }

        public bool TryCompleteTask(Guid id)
        {
            return TryUpdateTaskStatus(id, Status.Completed);
        }

        public bool TryDeleteTask(Guid id)
        {
            var task = _user.TodoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            _user.TodoList.Remove(task);
            return true;
        }

        public bool TryUpdateTaskName(Guid id, string name)
        {
            var task = _user.TodoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Name = name;
            task.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        public bool TryUpdateTaskDescription(Guid id, string descripiton)
        {
            var task = _user.TodoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Description = descripiton;
            task.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        public bool TryUpdateTaskStatus(Guid id, Status status)
        {
            var task = _user.TodoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }
            if (task.Status == status)
            {
                return true;
            }
            else if (task.Status is Status.Completed && status is Status.Current)
            {
                task.CompletedDate = null;
            }
            else if (task.Status is Status.Completed && status is Status.New)
            {
                task.CompletedDate = null;
                task.StartedDate = null;
            }
            else if (task.Status is Status.Current && status is Status.New)
            {
                task.StartedDate = null;
            }
            else if (task.Status is Status.Current && status is Status.Completed)
            {
                task.CompletedDate = DateTime.UtcNow;
            }
            else if (task.Status is Status.New && status is Status.Current)
            {
                task.StartedDate = DateTime.UtcNow;
            }
            else if (task.Status is Status.New && status is Status.Completed)
            {
                task.StartedDate = DateTime.UtcNow;
                task.CompletedDate = DateTime.UtcNow;
            }

            task.UpdatedDate = DateTime.UtcNow;
            task.Status = status;
            return true;
        }

        public List<Models.Task> GetTasks()
        {
            return _user.TodoList;
        }

        public Models.Task GetTask(Guid id)
        {
            return _user.TodoList.Find(t => t.Id == id);
        }
    }
}
