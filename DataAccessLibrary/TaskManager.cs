using DataAccessLibrary.Enums;
using DataAccessLibrary.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class TaskManager : ITaskManager
    {
        private Models.User _user;

        public TaskManager(Models.User user)
        {
            _user = user;
        }

        /// <summary>
        /// Creates a new task and adds it to the users Todo list.
        /// </summary>
        /// <param name="name">Name of the task.</param>
        /// <param name="description">Description of the task.</param>
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

        /// <summary>
        /// Updates the task status to "Current".
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryStartTask(Guid id)
        {
            return TryUpdateTaskStatus(id, Status.Current);
        }

        /// <summary>
        /// Updates the task status to "Completed".
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryCompleteTask(Guid id)
        {
            return TryUpdateTaskStatus(id, Status.Completed);
        }

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns>True if the task is found, false if not.</returns>
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

        /// <summary>
        /// Updates the name of the task.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <param name="name">Updated task name.</param>
        /// <returns>True if the task is found, false if not.</returns>
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

        /// <summary>
        /// Updates the name of the task.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <param name="descripiton">Updated task description.</param>
        /// <returns>True if the task is found, false if not.</returns>
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

        /// <summary>
        /// Updates the task status.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <param name="status">Updated task status.</param>
        /// <returns>True if the task is found, false if not.</returns>
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

        /// <summary>
        /// Gets all tasks from the user.
        /// </summary>
        /// <returns>User's TodoList.</returns>
        public List<Models.Task> GetTasks()
        {
            return _user.TodoList;
        }

        /// <summary>
        /// Gets a task from the user.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns></returns>
        public Models.Task GetTask(Guid id)
        {
            return _user.TodoList.Find(t => t.Id == id);
        }
    }
}
